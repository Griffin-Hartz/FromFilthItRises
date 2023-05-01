using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    //private Collider collider;
    [SerializeField] private Collider trigger;
    [SerializeField] private Material defaultMat;
    [SerializeField] private Material interactionMat;
    private MeshRenderer mesh;
    public Boolean needsToggled = false;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            //collider = GetComponent<Collider>();
            mesh = GetComponent<MeshRenderer>();
        }
        catch (NullReferenceException) { }

        defaultMat = Resources.Load<Material>("Default");
        interactionMat = Resources.Load<Material>("Interacting");
    }

    public virtual void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            //Debug.Log("Trigger");
            try { mesh.material = interactionMat; }
            catch { }

            if (needsToggled)
            {
                if (Input.GetKeyDown(KeyCode.E))
                    Interact();
            }
            else
                Interact();
        }
    }

    /*public virtual void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //Debug.Log("Collider");
            if (Input.GetKeyDown(KeyCode.E))
                Interact();
        }
    }*/

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            try { mesh.material = defaultMat; }
            catch { }
    }

    public virtual void Interact() { }
}
