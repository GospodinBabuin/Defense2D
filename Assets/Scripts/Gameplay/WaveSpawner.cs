using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private AudioSource _newWaveMusic;
    [System.Serializable]
    public class Wave
    {
        public string Name;
        public int Count = 3;
        public Transform[] Enemy;

    }
    public Wave[] Waves;
    public int waveType = 0;
    [SerializeField] private int[] daysWithNewWave; 
    private bool _isMonstersAppear = false;

    public Transform[] spawnPoints;
    public static WaveSpawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Instance = this;
        CheckDayWithNewWave();
    }

    private void FixedUpdate()
    {
        if (WorldManager.Instance.dayState == WorldManager.DayState.NIGHT && !_isMonstersAppear)
        {
            StartCoroutine(SpawnWave(Waves[waveType]));
        }
        if (WorldManager.Instance.dayState == WorldManager.DayState.DAY && _isMonstersAppear)
        {
            CheckDayWithNewWave();
            _isMonstersAppear = false;
        }
    }

    private void CheckDayWithNewWave()
    {
        bool needToPlayMusic = false;

        for (int i = 0; i < daysWithNewWave.Length; i++)
        {
            if (WorldManager.Instance.DayCount == daysWithNewWave[i])
            {
                waveType = i;
                needToPlayMusic = true;
            }
        }

        if (needToPlayMusic)
            _newWaveMusic.Play();
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log("spawning wave: " + wave.Name);
        _isMonstersAppear = true;

        for (int i = 0; i < wave.Count + WorldManager.Instance.DayCount; i++)
        {
            SpawnEnemy(wave.Enemy[Random.Range(0, wave.Enemy.Length)]);
            yield return new WaitForSeconds(1);
        }

        WorldManager.Instance.CanCheckIsMonstersDead = true;
        yield break;
    }

    private void SpawnEnemy(Transform enemy)
    {
        Debug.Log("spawning enemy: " + enemy.name);
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    }
    
    
    public bool IsMonstersDead()
    {
        if (WorldManager.Instance.CanCheckIsMonstersDead)
        {
            if (WorldManager.Instance.EnemyList.Count == 0)
                return true;
            else
                return false;
        }

        return false;
    }
}
