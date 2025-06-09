using System;
using UnityEngine;

namespace Script.BowAndArrow
{
    public class Arrow : MonoBehaviour
    {
        [Header("Stick Settings")]
        public float stickDepth = 0.1f; // How deep arrow penetrates
        private bool hasStuck = false;
        private Rigidbody rb;
        private Collider col;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
        }

        void OnCollisionEnter(Collision collision)
        {
            if (hasStuck) return;
        
            // Check if hit an enemy (modify tag as needed)
            if (collision.gameObject.CompareTag("Enemy")) 
            {
                StickArrow(collision);
            }
            else
            {
                // Destroy if hitting non-enemies (optional)
                Destroy(gameObject, 2f); 
            }
        }

        void StickArrow(Collision collision)
        {
            hasStuck = true;
        
            // 1. Disable physics
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        
            // 2. Parent to enemy
            transform.SetParent(collision.transform);
        
            // 3. Position at hit point with slight penetration
            ContactPoint contact = collision.contacts[0];
            transform.position = contact.point + contact.normal * -stickDepth;
        
            // 4. Rotate to match hit surface normal
            transform.rotation = Quaternion.LookRotation(-contact.normal);
        
            // 5. Disable collisions
            if (col != null) col.enabled = false;
        
            // 6. Optional: Add fixed joint for realistic movement
            /*FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = collision.rigidbody;*/
        }
    }
}