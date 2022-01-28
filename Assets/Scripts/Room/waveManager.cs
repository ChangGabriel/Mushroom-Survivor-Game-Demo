using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class waveManager : MonoBehaviour
{
    /*
     * Represents a single Enemy Wave
     */
    [System.Serializable]
    public class Wave
    {
        public string name;
        public int numberOfFrogs; //number of frogs to spawn
    }

    // Wave and enemy related
    private int numberOfEnemies;
    private int waveIndex;
    private int numberOfWaves;
    [SerializeField] private float waveTimer;
    private bool canSpawnWave;
    private bool waveOver;
    private GameObject waveGO;
    // Enemy references
    public Transform frog;
    public GameObject[] enemyPortals;
    public List<GameObject> activeSpawner;
    public Wave[] waves;

    // Event related
    public event EventHandler OnStartingEnemiesDead;
    //public event EventHandler OnBattleStarted;
    public event EventHandler OnBattleOver;

    // State related
    private State state;

    //Different states the room can be in
    private enum State
    {
        Normal,
        WaveActive,
        BattleOver,
    }

    private void Awake()
    {
        state = State.Normal;
    }
    // Start is called before the first frame update
    void Start()
    {
        OnStartingEnemiesDead += RoomManager_OnStartingEnemiesDead;
        //numberOfEnemies = listOfEnemies.transform.childCount;
        numberOfEnemies = 0;
        waveIndex = 0;
        numberOfWaves = waves.Length;
        canSpawnWave = true;
        for (int i = 0; i < enemyPortals.Length; i++)
        {
            activeSpawner.Add(enemyPortals[i]);
        }
    }

    //start wavebattle event
    private void RoomManager_OnStartingEnemiesDead(object sender, System.EventArgs e)
    {
            startWaveBattle();
            OnStartingEnemiesDead -= OnStartingEnemiesDead;
    }
    private void startWaveBattle()
    {
        state = State.WaveActive;
        //OnBattleStarted?.Invoke(this, EventArgs.Empty); //close entry portal
    }

    // Update is called once per frame
    void Update()
    {   
        //starts wavebattle if all starting enemies is dead
        if (numberOfEnemies == 0 && state == State.Normal)
        {
            OnStartingEnemiesDead?.Invoke(this, EventArgs.Empty); 
        }
        //start spawning waves from list and also check if wave is over to start next
        if (state == State.WaveActive && (canSpawnWave || waveOver))      
        {
            if (!isBattleOver()) //check if we have reached our number of waves for the room
            {
                StartCoroutine(spawnWave(waves[waveIndex]));
                StartCoroutine(waveSpawnTimer());
                canSpawnWave = false;
                waveOver = false;
                waveIndex++;
            }
        }
        // Victory, wavebattle is over. Open exit portal
        if(state == State.BattleOver)
        {
            OnBattleOver?.Invoke(this, EventArgs.Empty); 
        }

    }
   
    //handles check if battle is over
    private bool isBattleOver()
    {
        if(waveIndex < numberOfWaves)
        {
            return false;
        }
        state = State.BattleOver;
        return true;
    }

    public IEnumerator waveSpawnTimer()
    {
        for (int i = 0; i < waveTimer; i++)
        {
            yield return new WaitForSeconds(1f);
            if (waveOver) // Stop timer if wave is defeated early
            {
                yield break;
            }
        }
        canSpawnWave = true;
    }

    //handles spawning of one enemy. Spawns the enemy in parent gameObject
    private void spawnEnemy(Transform _enemy, GameObject parent)
    {
        Transform spawnPoint = activeSpawner[UnityEngine.Random.Range(0, activeSpawner.Count)].transform;
        var enemy = Instantiate(_enemy, spawnPoint.position, Quaternion.identity); // + (Vector3)UnityEngine.Random.insideUnitCircle*1.5f if we want to spawn within a radius
        enemy.transform.parent = parent.transform;
    }

    //handles the wave spawning. Sets the numberOfEnemies for the current wave and also creates wave parent gameObject for enemies to spawn in.
    private IEnumerator spawnWave(Wave _wave)
    {
        var waveGO = new GameObject();
        waveGO.transform.parent = gameObject.transform;
        waveGO.name = _wave.name;

        for (int i = 0; i < _wave.numberOfFrogs; i++)
        {
            spawnEnemy(frog, waveGO);
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.0f)); // Time between each spawn 
        }
        while(waveGO.transform.childCount > 0) // check if wave is done i.e. all enemies in the wave is dead
        {
            yield return new WaitForSeconds(1f); // check every second
        }
        yield return new WaitForSeconds(2f); //time to wait before next wave
        Destroy(waveGO);
        waveOver = true;

        yield break;
    }

    // handles spawners outside of level bounds. 
    public void toggleSpawner(bool outsideBounds, GameObject spawner)
    {
        if (outsideBounds)
        {
            activeSpawner.Remove(spawner);
        }
        else
        {
            activeSpawner.Add(spawner);
        }
    }
}
