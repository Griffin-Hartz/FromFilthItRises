using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Readable : Interactable
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
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
}
