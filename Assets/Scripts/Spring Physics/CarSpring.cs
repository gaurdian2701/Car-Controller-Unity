using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class CarSpring : MonoBehaviour
{
    [SerializeField] private Rigidbody mparentRigidbody;
    
    [SerializeField] private float mspringConstant = 1.0f; 
    [SerializeField] private float mspringRestLength = 1.0f;
    [SerializeField] private float mmaxSpringExtensionLength = 1.0f;
    [SerializeField] private float mdampeningValue = 1.0f; 
    [SerializeField] private float mwheelRadius = 1.0f;
    
    private Vector3 mtireProbePoint = Vector3.zero;
    private Vector3 mfinalWheelRestingPosition = Vector3.zero;
    private Vector3 mexpectedWheelRestingPosition = Vector3.zero;
    private RaycastHit springCastHitInfo;
    private bool mapplyRestorationForce = false;
    private Vector3 mrestorationForce = Vector3.zero;
    
    void Start()
    {
    }

    void FixedUpdate()
    {
        CalculateRestingPosition();
        ApplyRestorationForce();
    }

    void CalculateRestingPosition()
    {
        //Using raycasts to determine where to place the wheel
        if (Physics.Raycast(transform.position,- mparentRigidbody.transform.up, out springCastHitInfo,
                mspringRestLength + mmaxSpringExtensionLength + mwheelRadius))
        {
            mtireProbePoint = springCastHitInfo.point;
            mfinalWheelRestingPosition = springCastHitInfo.point + mwheelRadius * mparentRigidbody.transform.up;
            mapplyRestorationForce = true;
        }
        else
        {
            mtireProbePoint = transform.position - (mspringRestLength + mmaxSpringExtensionLength + mwheelRadius) * mparentRigidbody.transform.up;
            mfinalWheelRestingPosition = transform.position - (mspringRestLength + mmaxSpringExtensionLength) * mparentRigidbody.transform.up;
            mapplyRestorationForce = false;
            mrestorationForce =  Vector3.zero;
        }

        mexpectedWheelRestingPosition = transform.position - mparentRigidbody.transform.up * mspringRestLength;
    }

    void ApplyRestorationForce()
    {
        if (mapplyRestorationForce)
        {
            float displacement = Vector3.Distance(mexpectedWheelRestingPosition, mfinalWheelRestingPosition);

            mrestorationForce = mspringConstant * displacement * mparentRigidbody.transform.up;
            
            mparentRigidbody.AddForceAtPosition(mrestorationForce, springCastHitInfo.point);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.1f);

        Gizmos.color = Color.aquamarine;
        Gizmos.DrawSphere(mexpectedWheelRestingPosition, 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, mtireProbePoint);

        Gizmos.color = Color.orange;
        Gizmos.DrawWireSphere(mfinalWheelRestingPosition, mwheelRadius);
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(mtireProbePoint, new Vector3(0.1f, 0.1f, 0.1f));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + mrestorationForce);
    }
}
