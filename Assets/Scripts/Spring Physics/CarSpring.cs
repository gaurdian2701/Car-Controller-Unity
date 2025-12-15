using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.Serialization;

public class CarSpring : MonoBehaviour
{
    [SerializeField] private Rigidbody mparentRigidbody;

    #region wheel and spring properties

    [SerializeField] private float mspringConstant = 1.0f;
    [SerializeField] private float mspringRestLength = 1.0f;
    [SerializeField] private float mmaxSpringExtensionLength = 1.0f;
    [SerializeField] private float mspringDampingConstant = 1.0f;
    [SerializeField] private float mwheelRadius = 1.0f;

    private Vector3 mdebugTireProbePoint = Vector3.zero; //Tire probe for debugging purposes only
    private Vector3 mfinalWheelRestingPosition = Vector3.zero;
    private RaycastHit springCastHitInfo;

    #endregion

    #region physics and forces variables

    private bool mapplyRestorationForce = false;
    private Vector3 mrestorationForce = Vector3.zero;
    private Vector3 mspringVelocity = Vector3.zero; //This is the difference in LENGTH of the spring in a particular direction
    private float mcurrentSpringLength = 0.0f;
    #endregion

    void Start()
    {
    }

    void FixedUpdate()
    {
        CalculateWheelRestingPosition();
        ApplySpringForcesToParent();
    }

    void CalculateWheelRestingPosition()
    {
        //Using raycasts to determine where to place the wheel
        if (Physics.Raycast(transform.position, -mparentRigidbody.transform.up, out springCastHitInfo,
                mspringRestLength + mmaxSpringExtensionLength + mwheelRadius))
        {
            //If raycast hits the ground, place the wheel on the ground and apply lifting/spring forces
            //to the car since the spring "compresses"
            mdebugTireProbePoint = springCastHitInfo.point;
            mfinalWheelRestingPosition = springCastHitInfo.point + mwheelRadius * mparentRigidbody.transform.up;
            mcurrentSpringLength = springCastHitInfo.distance;
        }
        else
        {
            //Else, simply hang the wheel in the air and don't provide any lifting/spring forces to the car
            mdebugTireProbePoint = transform.position - (mspringRestLength + mmaxSpringExtensionLength + mwheelRadius)
                * mparentRigidbody.transform.up;
            mfinalWheelRestingPosition = transform.position -
                                         (mspringRestLength + mmaxSpringExtensionLength) *
                                         mparentRigidbody.transform.up;
            mcurrentSpringLength = mspringRestLength;
            mrestorationForce = Vector3.zero;
        }
    }

    //NOTE: ISOLATE SPRING LOGIC - IT DOES NOT CARE ABOUT WHEEL POSITIONS AND OUTSIDE FORCES. ONLY IT'S OWN LENGTH
    void ApplySpringForcesToParent()
    {
        //TODO: Calculate CHANGE in spring's length over time and take that as velocity to multiply with the damping constant
        float displacement = mspringRestLength - mcurrentSpringLength;
        mrestorationForce = (mspringConstant * displacement)
                            * mparentRigidbody.transform.up;
        mparentRigidbody.AddForceAtPosition(mrestorationForce, transform.position);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(mfinalWheelRestingPosition, mwheelRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, mdebugTireProbePoint);

        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(mdebugTireProbePoint, new Vector3(0.1f, 0.1f, 0.1f));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + mrestorationForce);
    }
}