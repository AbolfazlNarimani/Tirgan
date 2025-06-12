using System;
using UnityEngine;
using UnityEngine.Rendering;

public class UnitAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Start()
    {
        _animator.SetBool("MeleeAttack", !IsTargetRanged());
    }

    private void InstanceOnAllEnemiesDead(object sender, EventArgs e)
    {
        _animator.ResetTrigger("Shoot");
        Debug.Log("Trigger reset");
        _animator.SetBool("IsWallking", true);
    }

    private void InstanceOnShootStage(object sender, EventArgs e)
    {
       
    }

    public void SetTrigger(string trigger)
    {
        _animator.SetTrigger(trigger);
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
        Player.Instance.ShouldSpawnArrow = true;
    }
        
    public void OnReleaseArrow()
    {
        Player.Instance.CanReleaseArrow = true;
    }

    private bool IsTargetRanged()
    {
        return true;
    }

    public void SetBool(string boolName, bool value)
    {
        _animator.SetBool(boolName,value);
    }
}