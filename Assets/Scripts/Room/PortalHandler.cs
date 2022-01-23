using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This script handles the opening and closing of portals in a room 
 */
public class PortalHandler : MonoBehaviour
{
    [SerializeField] private GameObject portalExit;
    [SerializeField] private GameObject portalEntry;
    [SerializeField] private GameObject victoryMessage;
    [SerializeField] private waveManager waveManager;
    [SerializeField] private BossRoomManager bossRoomManager;



    // Material related
    private Material matActive;
    private Material matInactive;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer spEnemyPortal;

    // Start is called before the first frame update
    void Start()
    {
        //roomManager.OnBattleStarted += RoomManager_OnBattleStarted;
        if (waveManager != null)
        {
            waveManager.OnBattleOver += RoomManager_OnBattleOver;
        }
        if(bossRoomManager != null)
        {
            bossRoomManager.OnBattleOver += BossRoomManager_OnBattleOver;
        }
        if (portalExit != null)
        {
            spriteRenderer = portalExit.GetComponent<SpriteRenderer>();
        }

    }

    private void BossRoomManager_OnBattleOver(object sender, System.EventArgs e)
    {
        //Floor Cleared, do something
        if(victoryMessage != null)
        {
            victoryMessage.SetActive(true);
        }
        bossRoomManager.OnBattleOver -= BossRoomManager_OnBattleOver;
    }

    /*private void RoomManager_OnBattleStarted(object sender, System.EventArgs e)
    {
        //Close transition Portal that was open
        //Debug.Log("Entry Portal Closed");
        roomManager.OnBattleStarted -= RoomManager_OnBattleStarted;

    }*/

    private void RoomManager_OnBattleOver(object sender, System.EventArgs e)
    {
        //Open transition Portal so the player can go to the next room
        Debug.Log("Exit Portal Open");
        if (portalExit != null)
        {
            foreach (var enemyPortal in waveManager.enemyPortals)
            {
                spEnemyPortal = enemyPortal.GetComponent<SpriteRenderer>();
                spEnemyPortal.material = matInactive;
                spEnemyPortal.color = new Color32(37,37,63,105);
            }

            spriteRenderer.color = Color.white;
            spriteRenderer.material = matActive;
            portalExit.transform.GetChild(0).gameObject.SetActive(true);
            portalExit.transform.GetChild(1).gameObject.SetActive(true);

        }
        waveManager.OnBattleOver -= RoomManager_OnBattleOver;

    }

}
