using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class BossRoomManager : MonoBehaviour
{
    // Event references
    [SerializeField] private BossColliderTrigger colliderTrigger;
    public event EventHandler OnBattleOver;
    private State state;

    // Boss references
    [SerializeField] private GameObject bossList;
    private int numberOfEnemies;

    // Camera Related
    //[SerializeField] private CinemachineVirtualCamera playerCam;
    [SerializeField] private CinemachineVirtualCamera bossCam;

    [SerializeField] private GameObject nameplate;


    private enum State
    {
        PreBattle,
        BossBattle,
        BattleOver,
    }

    private void Awake()
    {
        state = State.PreBattle;
    }
    // Start is called before the first frame update
    void Start()
    {
        numberOfEnemies = bossList.transform.childCount;
        colliderTrigger.OnPlayerEnterTrigger += ColliderTrigger_OnPlayerEnterTrigger;
    }

    private void ColliderTrigger_OnPlayerEnterTrigger(object sender, System.EventArgs e)
    {
        if(state == State.PreBattle)
        {
            startBossBattle();
            colliderTrigger.OnPlayerEnterTrigger -= ColliderTrigger_OnPlayerEnterTrigger;
        }
    }

    //Pans the camera to show off the bosses
    private void startBossBattle()
    {
        StartCoroutine(panCameraToBoss());
        state = State.BossBattle;
    }

    private bool isBattleOver()
    {
        if (numberOfEnemies > 0)
        {
            return false;
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.BossBattle && isBattleOver())
        {
            state = State.BattleOver;
        }

        if (state == State.BattleOver)
        {
            OnBattleOver?.Invoke(this, EventArgs.Empty);
        }
    }

    private IEnumerator panCameraToBoss()
    {
        nameplate.SetActive(true);
        bossCam.Priority = 12;
        yield return new WaitForSeconds(3.0f);
        bossCam.gameObject.SetActive(false);

        //All bosses starts to attack after 4 sec
        yield return new WaitForSeconds(4.0f);
        foreach(Transform boss in bossList.transform)
        {
            //boss.GetComponent<Enemy>().aggravated = true;
        }

    }

    // This function is called from bosses-scripts when an boss dies
    public void decrementNumberOfEnemies()
    {
        numberOfEnemies--;
    }
}
