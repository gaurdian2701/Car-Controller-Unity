using System;
using UnityEngine;

namespace Car
{
    public class CarWheel : MonoBehaviour
    {
        [SerializeField] private Rigidbody mparentRigidbody; 
        [SerializeField] private GameObject mspring;
        [SerializeField] private GameObject mwheelMesh;
        
        public bool mshowSpringDebug = false;
        public bool mshowWheelDebug = false;

        #region suspension properties
        
        [Header("Spring Properties")]
        [SerializeField] private float mspringConstant = 1.0f;
        [SerializeField] private float mspringRestLength = 1.0f;
        [SerializeField] private float mspringDampingConstant = 1.0f;
        [SerializeField] private float mwheelRadius = 1.0f;

        private Vector3 mdebugTireProbePoint = Vector3.zero; //Tire probe for debugging purposes only
        private Vector3 mfinalWheelRestingPosition = Vector3.zero;
        private RaycastHit springCastHitInfo;
        
        #endregion
        
        #region physics and forces variables
        
        private Vector3 mrestorationForce = Vector3.zero;
        private float mcurrentSpringLength = 0.0f;
        private float moldSpringLength = 0.0f;
        private float springVelocity = 0.0f;
        
        #endregion
        
        #region wheel properties
        
        [Header("Wheel Properties")]
        public bool misTurningWheel = false;
        public bool misRearWheel = false;
        public bool misLeftWheel = false;
        
        #endregion

        void Start()
        {
            mcurrentSpringLength = mspringRestLength;
            moldSpringLength = mcurrentSpringLength;
        }

        void FixedUpdate()
        {
            ApplySpringForcesToParent();
        }

        private void Update()
        {
            CalculateWheelRestingPosition();
            mwheelMesh.transform.position = mfinalWheelRestingPosition;
            CalculateRestorationForce();
        }

        private void CalculateWheelRestingPosition()
        {
            //Using raycasts to determine where to place the wheel
            if (Physics.Raycast(mspring.transform.position, -mparentRigidbody.transform.up, out springCastHitInfo,
                    mspringRestLength + mwheelRadius))
            {
                //If raycast hits the ground, place the wheel on the ground and apply lifting/spring forces
                //to the car since the spring "compresses"
                mdebugTireProbePoint = springCastHitInfo.point;
                mfinalWheelRestingPosition = springCastHitInfo.point + mwheelRadius * mparentRigidbody.transform.up;
                mcurrentSpringLength = springCastHitInfo.distance;
            }
            else
            {
                //Else, simply hang the spring in the air at rest length
                mdebugTireProbePoint = mspring.transform.position -
                                       (mspringRestLength + mwheelRadius)
                                       * mparentRigidbody.transform.up;
                mfinalWheelRestingPosition = mwheelMesh.transform.position;
                mcurrentSpringLength = mspringRestLength;
            }
        }

        //NOTE: ISOLATE SPRING LOGIC - IT DOES NOT CARE ABOUT WHEEL POSITIONS AND OUTSIDE FORCES. ONLY IT'S OWN LENGTH
        private void CalculateRestorationForce()
        {
            springVelocity = mcurrentSpringLength - moldSpringLength;
            moldSpringLength = mcurrentSpringLength;
            
            //Calculate CHANGE in spring's length over time and take that as velocity to multiply with the damping constant
            float displacement = mspringRestLength - mcurrentSpringLength;
            mrestorationForce = (mspringConstant * displacement - springVelocity * mspringDampingConstant)
                                * mparentRigidbody.transform.up;
        }

        private void ApplySpringForcesToParent()
        {
            mparentRigidbody.AddForceAtPosition(mrestorationForce, mspring.transform.position);
        }
        
        void OnDrawGizmos()
        {
            if (mshowWheelDebug)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(mfinalWheelRestingPosition, mwheelRadius);
                
                Gizmos.color = Color.magenta;
                Gizmos.DrawCube(mdebugTireProbePoint, new Vector3(0.1f, 0.1f, 0.1f));
            }
            
            if (mshowSpringDebug)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(mspring.transform.position, 0.1f);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(mspring.transform.position, mdebugTireProbePoint);

                Gizmos.color = Color.blue;
                Gizmos.DrawLine(mspring.transform.position, transform.position + mrestorationForce);
            }
        }
    }
}

