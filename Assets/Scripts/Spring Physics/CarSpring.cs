using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class CarSpring : MonoBehaviour
{
    [SerializeField] private Rigidbody mparentRigidbody;
    
    [SerializeField] private float mspringConstant = 1.0f;
    [SerializeField] private float mrestLength = 1.0f;
    [SerializeField] private float mmaxSpringExtensionLength = 1.0f;
    [SerializeField] private float mdampeningValue = 1.0f; 
    [SerializeField] private float mwheelRadius = 1.0f;
    
    private Vector3 mtireProbePoint = Vector3.zero;
    private Vector3 mfinalWheelRestingPosition = Vector3.zero;
    
    void Start()
    {
    }

    void Update()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position,- mparentRigidbody.transform.up, out hitInfo,
                mrestLength + 2 * mwheelRadius))
        {
            mtireProbePoint = hitInfo.point;
            mfinalWheelRestingPosition =  hitInfo.point + mwheelRadius * mparentRigidbody.transform.up;
        }
        else
        {
            mtireProbePoint = transform.position - (mrestLength + mmaxSpringExtensionLength) * mparentRigidbody.transform.up;
            mfinalWheelRestingPosition = mtireProbePoint + mwheelRadius * mparentRigidbody.transform.up;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, mtireProbePoint);

        Gizmos.color = Color.orange;
        Gizmos.DrawWireSphere(mfinalWheelRestingPosition, mwheelRadius);
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(mtireProbePoint, new Vector3(0.1f, 0.1f, 0.1f));
    }
}
