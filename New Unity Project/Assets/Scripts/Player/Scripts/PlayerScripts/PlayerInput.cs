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

        #region MOVEMENT

        //MOVEMENT VECTOR
        _MovementForward = GetForwardMovement();
        //2D MOVEMENT
        PlayerManager._PlayerManager._playerMovement.SetMoveDirection(_MovementForward);

        // JUMP START
        if (Input.GetButtonDown("Jump"))
        {
            PlayerManager._PlayerManager._playerMovement.CheckJump();
        }
        // JUMP END
        if (Input.GetButtonUp("Jump"))
        {
            PlayerManager._PlayerManager._playerMovement.EndJump();
        }

        //RUN START
        if (Input.GetButtonDown("Fire3"))
        {
            PlayerManager._PlayerManager._playerMovement.Run();
        }
        //RUN END
        if (Input.GetButtonUp("Fire3"))
        {
            PlayerManager._PlayerManager._playerMovement.Walk();
        }

        #endregion
    }
    
}
