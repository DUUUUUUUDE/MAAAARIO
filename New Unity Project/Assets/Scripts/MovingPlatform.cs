using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public Transform m_CurrentPlatform;

    bool player;

    void AttachPlatform ()
    {
        if (!player)
        {
            PlayerManager.instace._Movement.transform.parent = transform.parent;
            player = true;
        }
    }

    void DetachPlatform()
    {
        if (player)
        {
            PlayerManager.instace._Movement.transform.parent = PlayerManager.instace.transform.parent;
            player = false;
        }
    }

    void Update()
    {

        if (m_CurrentPlatform != null)
        {

            if (m_CurrentPlatform.forward.y < 1)
                DetachPlatform();

        }

    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            AttachPlatform();
        }

    }

    public void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player")
        {
            DetachPlatform();
        }

    }
}
