using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gate : Interactable
{
    [SerializeField] private GameObject gate;
    [SerializeField] private TextMeshProUGUI tm;
    public override void Interact()
    {
        try
        {
            tm.gameObject.SetActive(false);
        }
        catch { }
        gate.SetActive(false);
    }

    public override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
    }
}
