using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JammedGate : Gate
{
    public override void Interact()
    {
        //base.Interact();
        gameObject.SetActive(false);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        //Impact by heavy object
        Debug.Log(collision.gameObject.name + "collision ");
        if(rb.mass >= 10 && rb.velocity.magnitude >= 1f)
        {
            Interact();
        }
    }
}
