using System;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float stickDepth = 0.1f;
    private bool hasStuck = false;
    private Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true; // Make collider a trigger
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasStuck) return;

        if (other.CompareTag("Enemy")) 
        {
            StickArrow(other);
            Player.Instance.HasHitEnemy = true;
        }
        else
        {
            Destroy(gameObject, 2f);
        }
    }

    void StickArrow(Collider enemyCollider)
    {
        hasStuck = true;
        col.enabled = false; // Disable further collisions

        // Parent to enemy
        transform.SetParent(enemyCollider.transform);

        // Stick arrow at contact point (approximate)
        // Since we don't have collision data, we can use raycasting or just stick it at the center
        transform.position = enemyCollider.bounds.center;
        
        // Optional: Rotate arrow to face the hit direction
        // transform.rotation = Quaternion.LookRotation(transform.forward);
    }
}