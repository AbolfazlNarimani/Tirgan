using System;
using UnityEngine;

namespace Script.BowAndArrow
{
    public class PlayerBow : MonoBehaviour
    {
           [Header("Arrow Settings")] 
        public GameObject arrowPrefab;
        public Transform arrowNockPoint;
        private GameObject currentArrow;
        private float shootPower = 40f;
        public static PlayerBow Instance { private set; get; }

        [Header("Targeting")]
        public Transform targetEnemy; // Assign this in inspector or find dynamically
        public float predictionFactor = 0.5f; // How much to lead the target
        public float heightOffset = 1.5f;

        private void Start()
        {
            Instance = this;
        }

        public void NockArrow()
        {
            if (currentArrow == null)
            {
                Quaternion arrowRotation = arrowNockPoint.rotation * Quaternion.Euler(90, 0, 0);
                currentArrow = Instantiate(arrowPrefab, arrowNockPoint.position, arrowRotation);
                currentArrow.transform.SetParent(arrowNockPoint);
            
                Rigidbody rb = currentArrow.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = false;

                currentArrow.transform.localPosition = Vector3.zero;
                currentArrow.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }

        public void ReleaseArrow()
        {
            
            if (currentArrow == null || targetEnemy == null) return;

            currentArrow.transform.SetParent(null);
            Rigidbody rb = currentArrow.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;

            // Calculate direction with prediction
            Vector3 enemyVelocity = targetEnemy.GetComponent<Rigidbody>()?.linearVelocity ?? Vector3.zero;
            Vector3 baseEnemyPos = targetEnemy.position;
            Vector3 offsetEnemyPos = baseEnemyPos + Vector3.up * heightOffset; // Apply height offset
            Vector3 predictedPos = offsetEnemyPos + enemyVelocity * predictionFactor;
            Vector3 shootDirection = (predictedPos - currentArrow.transform.position).normalized;

            rb.linearVelocity = shootDirection * shootPower;

            // Make arrow rotate towards velocity (optional)
            currentArrow.transform.up = shootDirection;

            if (!currentArrow.TryGetComponent<Arrow>(out _))
            {
                currentArrow.AddComponent<Arrow>();
            }
    
            currentArrow = null; // Release reference without destroying
        }

        // Call this to set target before shooting
        public void SetTarget(Transform enemy)
        {
            targetEnemy = enemy;
        }
    }
}