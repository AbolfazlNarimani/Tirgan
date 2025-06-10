using System;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Start()
    {
        if(TryGetComponent(out MovePlayerToNextWave movePlayerToNextWave));
        {
            movePlayerToNextWave.Move += MovePlayerToNextWaveOnmove;
        }
        if (IsTargetRanged())
        {
            _animator.SetBool("MeleeAttack",false);
        }
        else
        {
            _animator.SetBool("MeleeAttack", true);
        }
        Player.Instance.StopMoving += InstanceOnStopMoving;
        Player.Instance.ShootStage += InstanceOnShootStage;
    }

    private void InstanceOnAllEnemiesDead(object sender, EventArgs e)
    {
        _animator.ResetTrigger("Shoot");
        Debug.Log("Trigger reset");
        _animator.SetBool("IsWallking", true);
    }

    private void InstanceOnShootStage(object sender, EventArgs e)
    {
        Debug.Log("Animation Shoot");
        _animator.SetTrigger("Shoot");
    }

    private void InstanceOnStopMoving(object sender, EventArgs e)
    {
        _animator.SetBool("IsWallking", false);
    }


    private void MovePlayerToNextWaveOnmove(object sender, EventArgs e)
    {
        _animator.SetBool("IsWallking", true);
    }

    public void OnReachArrow()
    {
        Player.Instance.NockArrow(); // Prepare arrow visually
    }
        
    public void OnReleaseArrow()
    {
        Player.Instance.ReleaseArrow(); // Actually fire the arrow
    }

    private bool IsTargetRanged()
    {
        return true;
    }
}