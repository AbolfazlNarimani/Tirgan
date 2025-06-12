using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    private int WaveNumber = 1;


    public IEnumerator SpawnNextWave()
    {
        int enemyCount = 1;
        yield break;
    }

    

    public void UpdateWaveNumber()
    {
        
    }
}
