using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpringPhysics
{
    public class SimpleSpring : MonoBehaviour
    { 
        [SerializeField] private float mspringConstant = 1.0f;
        [SerializeField] private float mrestLength = 1.0f;
        [SerializeField] private float mdampeningValue = 1.0f;
        [SerializeField] private Rigidbody mattachedObjectRigidbody;
    
        private Vector3 mrestLengthPosition = Vector3.zero;
        private Vector3 mrestorationForce = Vector3.zero;

        void FixedUpdate()
        {
            CalculateRestLengthPosition();
            AddRestorationForceToAttachedObject();
        }

        void CalculateRestLengthPosition()
        {
            //First Calculate the current rest position in world space and use that to calculate the offset force needed to push the spring back to rest length
            mrestLengthPosition = transform.position - transform.up * mrestLength;
        }

        void AddRestorationForceToAttachedObject()
        {
            mrestorationForce = -mspringConstant * (mattachedObjectRigidbody.position - mrestLengthPosition)
                                - mattachedObjectRigidbody.linearVelocity * mdampeningValue;
        
            mattachedObjectRigidbody.AddForce(mrestorationForce);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.orange;
            Gizmos.DrawSphere(transform.position, 0.2f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(mrestLengthPosition, 0.2f);
        }
    }
}