using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Arrow Settings")] public GameObject arrowPrefab;
    public Transform arrowNockPoint;
    public float shootPower = 40f;
    public static Player Instance { private set; get; }

    public float predictionFactor = 0.5f;
    public float heightOffset = 1.5f;

    
    private HealthSystem _healthSystem;
    private UnitAnimation _animator;
    private EnemySpawner _enemySpawner;

    public bool CanReleaseArrow { get; set; }
    public bool ShouldSpawnArrow { get; set; }
    public bool HasHitEnemy { get; set; }
    private void Awake()
    {
        Instance = this;
        _animator = GetComponent<UnitAnimation>();
    }

    private void Start()
    {
        _animator.SetBool("RangeAttack",true);

        _healthSystem = GetComponent<HealthSystem>();
    }
    
    public IEnumerator StartShootingSequence()
    {
        MovePlayerToNextWave movePlayerToNextWave = GetComponent<MovePlayerToNextWave>();
        if (!movePlayerToNextWave.GetMoveInisialized())
        {
            yield return movePlayerToNextWave.InitialMove();
        }

        if (FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length == 0)
        {
           yield return movePlayerToNextWave.MoveToNextWave();
           yield return _enemySpawner.SpawnNextWave();
        }
        
        foreach (Enemy enemy in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            var enemyHealth = enemy.GetComponent<HealthSystem>();
            if (enemy == null || enemyHealth.IsDead()) 
                continue;
            
            
            _animator.SetBool("IsWallking",false);
            _animator.SetTrigger("Shoot");
            ShouldSpawnArrow = false;
            yield return new WaitUntil(() => ShouldSpawnArrow);
            
            
                
            var arrowRotation = arrowNockPoint.rotation * Quaternion.Euler(90, 0, 0);
            var currentArrow = Instantiate(arrowPrefab, arrowNockPoint.position, arrowRotation);
            currentArrow.transform.SetParent(arrowNockPoint);
            currentArrow.transform.localPosition = Vector3.zero;
            currentArrow.transform.localRotation = Quaternion.Euler(0, 0, 0);

            CanReleaseArrow = false;
            yield return new WaitUntil(() => CanReleaseArrow);
                
            yield return ReleaseArrow(enemy, currentArrow);

            HasHitEnemy = false;
            yield return new WaitUntil(() => HasHitEnemy);
            enemyHealth.TakeDamage(50);
            _animator.SetTrigger("RangeAttackDone");
            _animator.SetBool("RangeAttack",false);
        }
       
      
    }

    public IEnumerator ReleaseArrow(Enemy targetEnemy, GameObject currentArrow)
    {
        currentArrow.transform.SetParent(null);

        Vector3 enemyVelocity = targetEnemy.GetComponent<Rigidbody>()?.linearVelocity ?? Vector3.zero;
        Vector3 predictedPos = targetEnemy.transform.position + Vector3.up * heightOffset + enemyVelocity * predictionFactor;

        currentArrow.transform.DOMove(predictedPos, 0.5f).OnComplete(() =>
        {
            if (!currentArrow.TryGetComponent<Arrow>(out _))
            {
                currentArrow.AddComponent<Arrow>();
            }
        });

        currentArrow = null;
        yield break;
    }

    public void AttackPlayer(int damage)
    {
        _healthSystem.TakeDamage(damage);
    }

    /*private IEnumerator Felan()
    {
        Debug.Log("A");
        yield return Another();
        Debug.Log("C");
    }

    private IEnumerator Another()
    {
        Debug.Log("VAR");
        yield return new WaitForSeconds(10);
        Debug.Log("B");
    }*/
}