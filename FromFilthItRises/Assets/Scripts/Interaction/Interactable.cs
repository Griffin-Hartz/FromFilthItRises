using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private Collider trigger;
    [SerializeField] public Light lightSource;
    public Boolean needsToggled = false;
    public bool glowing;
    public bool needsLight;
    // Start is called before the first frame update

    public virtual void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if(needsLight)lightSource.enabled = true;

            if (needsToggled)
            {
                if (Input.GetKeyDown(KeyCode.E))
                    Interact();
            }
            else
                Interact();
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            if(needsLight)lightSource.enabled = false;
    }

    public virtual void Interact() { }

    public void StartGlow()
    {
        glowing = true;
        lightSource.enabled = true;
    }

    public void StopGlow()
    {
        glowing = false;
        lightSource.enabled = false;
    }
}

