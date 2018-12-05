using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformPhysics : MonoBehaviour {


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<HingeJoint>())
        {
            Debug.Log("X");
            other.gameObject.GetComponent<Rigidbody>().AddForce(transform.position + -transform.up * 10000 , ForceMode.Impulse);
        }
    }

}
