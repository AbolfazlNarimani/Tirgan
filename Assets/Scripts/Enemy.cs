using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void Start()
    {
        GetComponent<HealthSystem>().OnDead += OnOnDead;
    }

    private void OnOnDead(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }
}