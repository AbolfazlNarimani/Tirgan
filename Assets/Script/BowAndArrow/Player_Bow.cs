using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Script.BowAndArrow
{
    public class PlayerBow : MonoBehaviour
    {
        [Header("Arrow Settings")] public GameObject arrowPrefab;
        public Transform arrowNockPoint;
        private GameObject currentArrow;
        private float shootPower = 40f;
        public static PlayerBow Instance { private set; get; }

        [Header("Targeting")] public List<Enemy> targetEnemyList; // Assign this in inspector or find dynamically
        public float predictionFactor = 0.5f; // How much to lead the target
        public float heightOffset = 1.5f;

        private List<Enemy> _liveEnemy;
        private List<Enemy> _deadEnemy;

        private void Start()
        {
            Instance = this;
            _liveEnemy = new List<Enemy>();
            _deadEnemy = new List<Enemy>();
        }

        public void NockArrow()
        {
            if (currentArrow == null)
            {
                Quaternion arrowRotation = arrowNockPoint.rotation * Quaternion.Euler(90, 0, 0);
                currentArrow = Instantiate(arrowPrefab, arrowNockPoint.position, arrowRotation);
                currentArrow.transform.SetParent(arrowNockPoint);
                currentArrow.transform.localPosition = Vector3.zero;
                currentArrow.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }

        private int _index = 0;
        public void ReleaseArrow()
        {
            foreach (var enemy in targetEnemyList)
            {
                if (_deadEnemy.Contains(enemy))
                {
                    continue;
                }
                _liveEnemy.Add(enemy);
            }
            currentArrow.transform.SetParent(null);
          
            Enemy targetEnemy = targetEnemyList[_index];
                // Calculate direction with prediction
                Vector3 enemyVelocity = targetEnemy.GetComponent<Rigidbody>()?.linearVelocity ?? Vector3.zero;
                Vector3 baseEnemyPos = targetEnemy.transform.position;
                Vector3 offsetEnemyPos = baseEnemyPos + Vector3.up * heightOffset; // Apply height offset
                Vector3 predictedPos = offsetEnemyPos + enemyVelocity * predictionFactor;
                currentArrow.transform.DOMove(predictedPos, 1f);
                _deadEnemy.Add(targetEnemy);
                _index = (_index + 1) % _liveEnemy.Count;
            
            if (!currentArrow.TryGetComponent<Arrow>(out _))
            {
                currentArrow.AddComponent<Arrow>();
            }

            currentArrow = null; // Release reference without destroying
        }
    }
}