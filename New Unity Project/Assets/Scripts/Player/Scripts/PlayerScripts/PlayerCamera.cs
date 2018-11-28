using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    public float _MouseSensitivity;
    protected float _XAxisClamp = 0.0f;
    CharacterController _CharacterController;
    protected Camera _CharacterCamera;


    public bool m_AngleLocked;

    public float m_YawRotationalSpeed;
    public float m_PitchRotationalSpeed;

    public float m_MinPitch;
    public float m_MaxPitch;

    public Transform m_LookAt;

    public float m_DistanceToLookAt;
    public float m_MinDistanceToLookAt;

    public LayerMask m_RaycastLayerMask;
    public float m_OffsetOnCollision;

    float l_Distance;
    Vector3 l_Direction;

    Vector3 LastDirectionVector;

    private void Start()
    {
        _CharacterCamera = Camera.main;
        l_Distance = m_DistanceToLookAt;
        l_Direction = m_LookAt.position - transform.position;
        l_Direction = l_Direction.normalized;
        LastDirectionVector = l_Direction;
    }

    private void LateUpdate()
    {
        MoveCamera();
    }

    void MoveCamera()

    {

        //..

        float l_MouseAxisX = Input.GetAxis("Mouse X");

        float l_MouseAxisY = Input.GetAxis("Mouse Y");

        //..

        Vector3 l_DesiredPosition = transform.position;

        if (!m_AngleLocked &&(l_MouseAxisX > 0.01f || l_MouseAxisX < -0.01f || l_MouseAxisY > 0.01f || l_MouseAxisY < -0.01f))

        {

            Vector3 l_EulerAngles = transform.eulerAngles;

            float l_Yaw = (l_EulerAngles.y + 180.0f);

            float l_Pitch = l_EulerAngles.x;

            l_Yaw += m_YawRotationalSpeed * l_MouseAxisX * Time.deltaTime;

            l_Yaw *= Mathf.Deg2Rad;

            if (l_Pitch > 180.0f)

                l_Pitch -= 360.0f;

            l_Pitch += m_PitchRotationalSpeed * (-l_MouseAxisY) * Time.deltaTime;

            l_Pitch = Mathf.Clamp(l_Pitch, m_MinPitch, m_MaxPitch);

            l_Pitch *= Mathf.Deg2Rad;

            l_DesiredPosition = m_LookAt.position + new Vector3(Mathf.Sin(l_Yaw) * Mathf.Cos(l_Pitch) * l_Distance, Mathf.Sin(l_Pitch) * l_Distance, Mathf.Cos(l_Yaw) * Mathf.Cos(l_Pitch) * l_Distance);

            l_Direction = m_LookAt.position - l_DesiredPosition;

            LastDirectionVector = l_Direction.normalized;
        }
        else
        {

            l_Direction = m_LookAt.position - transform.position;
            l_Direction = l_Direction.normalized;
            l_Direction.y = LastDirectionVector.y;

            l_DesiredPosition = transform.position;

        }


        l_Direction = m_LookAt.position - l_DesiredPosition;
        l_Direction.Normalize();

        l_Distance = (transform.position - m_LookAt.position).magnitude;


        if (l_Distance > m_DistanceToLookAt)
        {
            if (l_Direction.y > 0.1)

            l_DesiredPosition = m_LookAt.position - l_Direction * m_DistanceToLookAt;

        }
        else if (l_Distance < m_MinDistanceToLookAt)
        {

            l_DesiredPosition = m_LookAt.position - l_Direction * m_MinDistanceToLookAt;

        }



        RaycastHit l_RaycastHit;

        Ray l_Ray = new Ray(m_LookAt.position, -l_Direction);

        if (Physics.Raycast(l_Ray, out l_RaycastHit, l_Distance, m_RaycastLayerMask.value))

            l_DesiredPosition = l_RaycastHit.point + l_Direction * m_OffsetOnCollision;

        transform.forward = l_Direction;

        transform.position = l_DesiredPosition;

    }
}
