using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(Rigidbody))] 
public class PickUp : Interactable
{
    public override void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            StartGlow();

            if (needsToggled)
            {
                if (Input.GetKeyDown(KeyCode.E))
                    Interact();
            }
            else
                Interact();
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            StopGlow();
    }

    public override void Interact()
    {
        StopGlow();
    }
}
