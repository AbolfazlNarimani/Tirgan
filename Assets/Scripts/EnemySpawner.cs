using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] waveOnePrefab;
    [SerializeField] private Transform[] waveTowPrefab;
    [SerializeField] private Transform[] waveThreePrefab;
    [SerializeField] private List<Transform> platforms;
    private int _waveNumber = 0;

    public static EnemySpawner Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator SpawnNextWave()
    {
        if (_waveNumber >= platforms.Count)
        {
            Debug.Log("GameOver");
            yield break;
        }

        if (SpawnWaveEnemies())
        {
            Debug.Log("EnemySpawned");
            yield return Player.Instance.StartShootingSequence();
        }

        yield break;
    }

    private float _spawnXPosition = -2f;
    private int spawnZPosition = 28;

    private bool SpawnWaveEnemies()
    {
        if (_waveNumber == 0)
        {
            var platform = platforms[_waveNumber];

            foreach (var enemy in waveOnePrefab)
            {
                Vector3 spawnPosition = new Vector3(
                    _spawnXPosition,
                    platform.position.y,
                    spawnZPosition
                );

                Instantiate(enemy, spawnPosition, Quaternion.identity);
                _spawnXPosition++;
            }

            _spawnXPosition = -2f;
            _waveNumber++;
            return true;
        }

        if (_waveNumber == 1)
        {
            var platform = platforms[_waveNumber];
            foreach (var enemy in waveOnePrefab)
            {
                Vector3 spawnPosition = new Vector3(
                    _spawnXPosition,
                    platform.position.y,
                    spawnZPosition
                );

                Instantiate(enemy, spawnPosition, Quaternion.identity);
                _spawnXPosition++;
            }

            _spawnXPosition = -2f;
            _waveNumber++;
            return true;
        }

        if (_waveNumber == 2)
        {
            var platform = platforms[_waveNumber];
            foreach (var enemy in waveOnePrefab)
            {
                Vector3 spawnPosition = new Vector3(
                    _spawnXPosition,
                    platform.position.y,
                    spawnZPosition
                );

                Instantiate(enemy, spawnPosition, Quaternion.identity);
                _spawnXPosition++;
            }

            _spawnXPosition = -2f;
            _waveNumber++;
            return true;
        }

        return false;
    }
}