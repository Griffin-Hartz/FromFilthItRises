using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : Interactable
{
    [SerializeField] private GameObject gate;
    public override void Interact()
    {
        Debug.Log("Interact");
        gate.SetActive(false);
    }

    public override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Trigger");
        }
    }
}
