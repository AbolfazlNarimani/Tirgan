using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [FormerlySerializedAs("_player")] [SerializeField]
    private Player player;

    [FormerlySerializedAs("playerMoveSpeed")] [SerializeField]
    private int attackRage;

    [FormerlySerializedAs("attackRange")] [SerializeField]
    private float enemyMoveSpeed;

    private UnitAnimation animator;

    private void Awake()
    {
        animator = GetComponent<UnitAnimation>();
    }

    private void Start()
    {
        GetComponent<HealthSystem>().OnDead += OnOnDead;
    }

    public IEnumerator ProcessEnemyActions(Enemy enemy)
    {
        if (enemy == null || enemy.GetComponent<HealthSystem>().IsDead()) yield break;

        if (ShouldMoveForward())
        {
            animator.SetTrigger("NormalEnemy");
            animator.SetBool("IsWallking", true);
            enemy.transform.DOMoveZ(transform.position.z - enemyMoveSpeed, 1f);
        }
        else
        {
            animator.SetTrigger("Slash");
            yield return AttackThePlayer();
        }

        yield return new WaitForSeconds(1f);
    }

    private IEnumerator AttackThePlayer()
    {
        player.AttackPlayer(10);
        yield return new WaitForSeconds(.5f);
    }

    private bool ShouldMoveForward()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRage)
        {
            return true;
        }

        return false;
    }

    private void OnOnDead(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }
}