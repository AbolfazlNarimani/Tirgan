using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{

    [FormerlySerializedAs("_player")] [SerializeField] private Player player;
    [FormerlySerializedAs("playerMoveSpeed")] [SerializeField] private float enemyMoveSpeed;

    private UnitAnimation animator;

    private void Awake()
    {
        animator = GetComponent<UnitAnimation>();
    }

    private void Start() 
    {
        GetComponent<HealthSystem>().OnDead += OnOnDead;
    }
    
    public IEnumerator ProcessEnemyActions()
    {
        Enemy[] enemyList = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemyList)
        {
            if (enemy == null || enemy.GetComponent<HealthSystem>().IsDead()) continue;
            animator.SetBool("IsWallking",false);
            animator.SetBool("MeleeAttack",true);
            if (TryGettingToThePlayer())
            {
                animator.SetBool("IsWallking",true);
                enemy.transform.DOMove(player.transform.position, 3f);
                
            }
            else
            {
               animator.SetTrigger("Slash");
               yield return AttackThePlayer();
            }
        }
    }

    private bool TryGettingToThePlayer()
    {
        if (transform.position.z + 2 != player.transform.position.z)
        {
            transform.DOMoveZ(transform.position.z + enemyMoveSpeed, 1f);
            return true;
        }

        return false;
    }

    private IEnumerator AttackThePlayer()
    {
      // player.AttackPlayer(enemyDamage);
        yield break;
    }

    private void OnOnDead(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }
    
}