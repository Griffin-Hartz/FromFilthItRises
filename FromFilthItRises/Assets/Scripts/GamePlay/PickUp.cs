using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(Rigidbody))] 
public class PickUp : Interactable
{
    [SerializeField] private Transform spawn;

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
        else if(other.tag == "ItemRespawn")
        {
            Respawn();
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            StopGlow();
    }

    /*private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            GetComponent<SphereCollider>().enabled = true;
    }*/

    public override void Interact()
    {
        StopGlow();
    }

    private void Respawn()
    {
        this.transform.position = spawn.position;
        this.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
    }
}
