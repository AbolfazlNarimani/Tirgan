using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class MovePlayerToNextWave : MonoBehaviour
{
    [SerializeField] private Vector3 zMoveAmount;
    [SerializeField] private Vector3 initialMoveValue;

    private bool MoveInisialized = false;
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

    public IEnumerator MoveToNextWave()
    {
        animator.SetBool("IsWallking",true);
        transform.DOMoveZ(zMoveAmount.z, 5f);
        yield return EnemySpawner.Instance.SpawnNextWave();
    }

    public IEnumerator InitialMove()
    {
        animator.SetBool("IsWallking",true);
        transform.DOMoveZ(initialMoveValue.z, 2f);
        MoveInisialized = true;
        yield return EnemySpawner.Instance.SpawnNextWave();
    }

    public bool GetMoveInisialized() => MoveInisialized;
}