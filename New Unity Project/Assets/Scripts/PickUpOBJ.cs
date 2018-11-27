using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpOBJ : MonoBehaviour , IRestartGameElement
{

    public void Start()
    {
        GameManager.instance.AddRestartGameElement(this);
    }

    public void RestartGame()
    {
        gameObject.SetActive(true);
    }

    public virtual void PickUp ()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        PickUp();
        gameObject.SetActive(false);
    }

}
