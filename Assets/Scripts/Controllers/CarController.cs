using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Car
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarController : MonoBehaviour
    {
        public bool mshowDebug = false;
        
        [Header("Car Steering Properties - Default values are from Ford Mustang 5th gen")]
        [SerializeField] private float mwheelBaseLength = 2.72f;
        [SerializeField] private float mturnRadius = 11.5f;
        [SerializeField] private float mrearTrackLength = 1.6f;
        
        [Header("Car Wheels")]
        [SerializeField] private CarWheel mfrontLeftWheel;
        [SerializeField] private CarWheel mfrontRightWheel;
        [SerializeField] private CarWheel mrearLeftWheel;
        [SerializeField] private CarWheel mrearRightWheel;
        
        private float msteerInput = 0.0f;
        private float msteerAngle = 0.0f;

        void Awake()
        {
        }
        private void Start()
        {
        }

        public void ReceiveInput(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            
            if (input.x > 0)
            {
                msteerInput = 1.0f;
            }
            else if (input.x < 0)
            {
                msteerInput = -1.0f;
            }
        }

        private void Update()
        {
            msteerAngle = Mathf.Rad2Deg * Mathf.Atan2(mwheelBaseLength, mturnRadius + mrearTrackLength / 2) * msteerInput;
            mfrontLeftWheel.transform.localRotation = Quaternion.AngleAxis(msteerAngle, Vector3.up);
            mfrontRightWheel.transform.localRotation = Quaternion.AngleAxis(msteerAngle, Vector3.up);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Handles.Label(mfrontLeftWheel.transform.position, "steer angle: " + msteerAngle);
            Handles.Label(mfrontRightWheel.transform.position, "steer angle: " + msteerAngle);
        }
    }
}
