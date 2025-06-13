using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class UnitAnimation : MonoBehaviour
{
    [FormerlySerializedAs("_animator")] [SerializeField] private Animator animator;
    
    public void SetTrigger(string trigger)
    {
        animator.SetTrigger(trigger);
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
        animator.SetBool(boolName,value);
    }
}