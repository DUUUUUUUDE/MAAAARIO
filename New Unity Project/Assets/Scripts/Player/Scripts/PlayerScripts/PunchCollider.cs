using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchCollider : MonoBehaviour {

    private void OnEnable()
    {
        StartCoroutine(disapearCO());
        Debug.Log("x");
    }

    IEnumerator disapearCO ()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goomba")
        {
            other.gameObject.SetActive(false);
        }
    }
}
