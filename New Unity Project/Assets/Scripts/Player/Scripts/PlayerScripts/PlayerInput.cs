using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    [HideInInspector]
    public Vector3 _MovementForward;

    Vector3 GetForwardMovement()
    {

        Vector3 l_Forward = Camera.main.transform.forward.normalized;
        Vector3 l_Right = Camera.main.transform.right.normalized;

        l_Forward *= Input.GetAxis("Vertical");
        l_Right *= Input.GetAxis("Horizontal");

        Vector3 dir = l_Forward + l_Right;

        if (dir.magnitude > 1)
            return dir.normalized;
        else
            return dir;
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlayerManager.instace._Movement.CheckPunch();
        }

        #region MOVEMENT

        //MOVEMENT VECTOR
        _MovementForward = GetForwardMovement();
        //2D MOVEMENT
        PlayerManager.instace._Movement.SetMoveDirection(_MovementForward);

        // JUMP START
        if (Input.GetButtonDown("Jump"))
        {
            PlayerManager.instace._Movement.CheckJump();
        }
        // JUMP END
        if (Input.GetButtonUp("Jump"))
        {
            PlayerManager.instace._Movement.EndJump();
        }

        //RUN START
        if (Input.GetButtonDown("Fire3"))
        {
            PlayerManager.instace._Movement.Run();
        }
        //RUN END
        if (Input.GetButtonUp("Fire3"))
        {
            PlayerManager.instace._Movement.Walk();
        }

        #endregion
    }
    
}
