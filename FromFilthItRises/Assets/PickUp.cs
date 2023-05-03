using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private Light lightSource;
    public bool glowing;

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
