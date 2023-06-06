using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class CarryingUI : MonoBehaviour
{
    [SerializeField] private Player player;
    public UnityEvent<PickUp> PickedUp;
    public TextMeshProUGUI _textMeshPro;

    private void Start()
    {
        PickedUp.AddListener(UpdatePickupUI);
        _textMeshPro= GetComponent<TextMeshProUGUI>();
    }

    private void UpdatePickupUI(PickUp pu)
    {
        //Debug.Log("UpdatePickupUI: " + pu.name);
        if (player.isCarrying)
        {
            _textMeshPro.enabled = true;
            _textMeshPro.text = "Carrying " + pu.gameObject.name;
        }
        else
        {
            _textMeshPro.enabled = false;
        }
    }
}
