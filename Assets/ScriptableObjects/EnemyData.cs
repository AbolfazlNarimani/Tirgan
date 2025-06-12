using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemies/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public GameObject prefab;
        public float health;
        public float damage;
        public float moveSpeed;
        public bool isRange;
    }
}
