using System.Collections.Generic;
using UnityEngine;

namespace Car
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarController : MonoBehaviour
    {
        [Header("Car Steering Properties - Default values are from Ford Mustang 5th gen")]
        [SerializeField] private float mwheelBaseLength = 2.72f;
        [SerializeField] private float mturnRadius = 11.5f;
        [SerializeField] private float mrearTrackLength = 1.6f;
        [SerializeField] private List<CarWheel> mwheels;

        private List<CarWheel> mrearWheels;
        private float mturnInput = 0.0f;
        private Rigidbody mcarRigidBody;
        
        private void Start()
        {
            foreach (CarWheel wheel in mwheels)
            {
                if (wheel.misRearWheel)
                {
                    mrearWheels.Add(wheel);
                }
            }
        }

        private void Update()
        {
        }

        void OnDrawGizmos()
        {
        }
    }
}
