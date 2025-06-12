using System;
using DG.Tweening;
using UnityEngine;

public class MovePlayerToNextWave : MonoBehaviour
{
    [SerializeField] private Vector3 zMoveAmount;
    [SerializeField] private Vector3 initialMoveValue;
    public event EventHandler Move;
    private UnitAnimation animator;

    private void Awake()
    {
        animator = GetComponent<UnitAnimation>();
    }

    private void Start()
    {
        InitialMove();
    }

    private void InstanceOnEnemiesDied(object sender, EventArgs e)
    {
        Move?.Invoke(this,EventArgs.Empty);
        transform.DOMoveZ(zMoveAmount.z, 10f);
    }

    public void InitialMove()
    {
        transform.DOMoveZ(initialMoveValue.z, 2f);
        animator.SetBool("IsWallking",false);
    }
}