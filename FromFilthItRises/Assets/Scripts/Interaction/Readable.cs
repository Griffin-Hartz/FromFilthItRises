using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Readable : Interactable
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    public bool print = false;
    
    public override void Interact()
    {
        _textMeshPro.gameObject.SetActive(true);
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.gameObject.tag == "Player")
            _textMeshPro.gameObject.SetActive(false);
    }

    public override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
        //if(print && other.tag == "Player") Debug.Log("OnTriggerStay");
    }
}
