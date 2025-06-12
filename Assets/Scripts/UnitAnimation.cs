using System;
using UnityEngine;
using UnityEngine.Rendering;

public class UnitAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    public void SetTrigger(string trigger)
    {
        _animator.SetTrigger(trigger);
    }
    
    public void OnReachArrow()
    {
        Player.Instance.ShouldSpawnArrow = true;
    }
        
    public void OnReleaseArrow()
    {
        Player.Instance.CanReleaseArrow = true;
    }
    

    public void SetBool(string boolName, bool value)
    {
        _animator.SetBool(boolName,value);
    }
}