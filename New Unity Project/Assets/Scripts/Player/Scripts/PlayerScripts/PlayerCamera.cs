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

    public LayerMask m_RaycastLayerMask;
    public float m_OffsetOnCollision;

    float l_Distance;
    Vector3 l_Direction;

    private void Start()
    {
        _CharacterCamera = Camera.main;
        l_Distance = m_DistanceToLookAt;
    }

    private void LateUpdate()
    {
        //RotateCamera();
        MoveCamera();
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float rotAmountX = mouseX * _MouseSensitivity;
        float rotAmountY = mouseY * _MouseSensitivity;

        _XAxisClamp -= rotAmountY;

        Vector3 targetRotCam = _CharacterCamera.transform.rotation.eulerAngles;
        Vector3 targetRotBody = transform.rotation.eulerAngles;

        targetRotCam.x -= rotAmountY;
        targetRotCam.z = 0;
        targetRotBody.y += rotAmountX;

        if (_XAxisClamp > 90)
        {
            _XAxisClamp = 90;
            targetRotCam.x = 90;
        }
        else if (_XAxisClamp < -90)
        {
            _XAxisClamp = -90;
            targetRotCam.x = 270;
        }

        _CharacterCamera.transform.rotation = Quaternion.Euler(new Vector3(targetRotCam.x, targetRotCam.y, transform.rotation.eulerAngles.z));
        transform.rotation = Quaternion.Euler(new Vector3(targetRotBody.x, targetRotBody.y, transform.rotation.eulerAngles.z));

    }

    void MoveCamera()

    {

        //..

        float l_MouseAxisX = Input.GetAxis("Mouse X");

        float l_MouseAxisY = Input.GetAxis("Mouse Y");

        //..

        Vector3 l_DesiredPosition = transform.position;

        if ((l_MouseAxisX > 0.01f || l_MouseAxisX < -0.01f || l_MouseAxisY > 0.01f || l_MouseAxisY < -0.01f))

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

        }
        else
        {
            
            l_Direction = m_LookAt.position - transform.position;

        }

        l_Direction /= l_Distance;


        if (l_Distance > m_DistanceToLookAt)

        {

            l_DesiredPosition = m_LookAt.position - l_Direction * m_DistanceToLookAt;

            l_Distance = m_DistanceToLookAt;

        }



        RaycastHit l_RaycastHit;

        Ray l_Ray = new Ray(m_LookAt.position, -l_Direction);

        if (Physics.Raycast(l_Ray, out l_RaycastHit, l_Distance, m_RaycastLayerMask.value))

            l_DesiredPosition = l_RaycastHit.point + l_Direction * m_OffsetOnCollision;

        transform.forward = l_Direction;

        transform.position = l_DesiredPosition;

    }
}
