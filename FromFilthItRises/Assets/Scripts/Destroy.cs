using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public BoxCollider collider;

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if(other.transform.tag == "Player")
        {
            collider.gameObject.SetActive(false);
        }
    }

}
