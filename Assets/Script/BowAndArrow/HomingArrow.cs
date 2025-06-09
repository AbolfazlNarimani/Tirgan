using UnityEngine;

namespace Script.BowAndArrow
{
    public class HomingArrow : MonoBehaviour
    {
        public Transform target;
        public float homingStrength = 5f;
        public float speed = 20f;
        private Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.linearVelocity = transform.forward * speed;
        }

        void FixedUpdate()
        {
            if (target != null)
            {
                Vector3 desiredDirection = (target.position - transform.position).normalized;
                Vector3 steeringForce = (desiredDirection - rb.linearVelocity.normalized) * homingStrength;
                rb.linearVelocity += steeringForce * Time.fixedDeltaTime;
                transform.forward = rb.linearVelocity.normalized;
            }
        }
    }
}
