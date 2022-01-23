using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransfer : MonoBehaviour
{
    private GameObject toolTip;
    private Transform nextRoomPortalPos;
    private bool activated;
    private Transform player;
    public float percentageMaxHpRestore;

    public GameObject currentRoom;
    public GameObject nextRoom;
    public GameObject vcamFade;

    // Start is called before the first frame update
    void Start()
    {
        activated = false;
        toolTip = gameObject.transform.GetChild(0).gameObject;
        nextRoomPortalPos = nextRoom.gameObject.transform.GetChild(1).gameObject.transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        //FOR DEMO PURPOSE ONLY, TELEPORT PLAYER TO NEXT ROOM
        if (Input.GetKeyDown("p"))
        {
            StartCoroutine(enableNextRoomCamera());
        }
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.CompareTag("Player")) 
        {
            if (Input.GetKey(KeyCode.E) && !activated)
            {
                activated = true;
                StartCoroutine(enableNextRoomCamera());
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            toolTip.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            toolTip.SetActive(false);
        }
    }

    private IEnumerator enableNextRoomCamera()
    {
        yield return new WaitForSeconds(0.5f);
        vcamFade.SetActive(true);
        yield return new WaitForSeconds(1.1f);
        currentRoom.SetActive(false);
        vcamFade.SetActive(false);
        nextRoom.SetActive(true);
        player.position = nextRoomPortalPos.position; //Teleport the player to next room
    }
}
