using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private const float HealthMax = 100f;
    [SerializeField] private int health = 100;
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;

    private bool _isDead = false;

    public void TakeDamage(int damage)
    {
        health -= damage;
        OnDamaged?.Invoke(this, EventArgs.Empty);
        if (health < 0)
        {
            health = 0;
        }

        if (health <= 0)
        {
            Die();
        }

        if (health > 0)
        {
            _isDead = false;
        }
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
        _isDead = true;
    }

    public float GetHealthNormalized()
    {
        return health / HealthMax;
    }

    public bool IsDead()
    {
        return _isDead;
    }
}