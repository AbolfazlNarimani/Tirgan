using System;
using DG.Tweening;
using UnityEngine;

public class MovePlayerToNextWave : MonoBehaviour
{
    [SerializeField] private Vector3 zMoveAmount;
    [SerializeField] private Vector3 initialMoveValue;
    public event EventHandler Move;

    private void Start()
    {
        Player.Instance.OnEnemiesDied += InstanceOnEnemiesDied;
        InitialMove();
    }

    private void InstanceOnEnemiesDied(object sender, EventArgs e)
    {
        Move?.Invoke(this,EventArgs.Empty);
        transform.DOMoveZ(zMoveAmount.z, 10f);
    }

    public void InitialMove()
    {
        Move?.Invoke(this,EventArgs.Empty);
        transform.DOMoveZ(initialMoveValue.z, 2f);
    }
}