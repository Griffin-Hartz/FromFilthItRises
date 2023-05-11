using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Interaction")]
    public Vector3 objectPosition;
    [SerializeField] private float throwForce = 1f;
    [SerializeField] private float floatOffset = 1f;
    [SerializeField] private float grabDistance = 1f;
    [SerializeField] private bool isCarrying = false;
    [Header("Movement")]
    [SerializeField] private float regMoveSpeed = 4f;
    [SerializeField] private float slowedMoveSpeed = 2f;
    [SerializeField] private Transform spawn;
    private Ray lookRay;
    private RaycastHit hitData;
    private PickUp carriedItem;
    private PickUp lastSeenPickup;
    private FirstPersonController firstPersonController;

    private void Start()
    {
        firstPersonController = GetComponent<FirstPersonController>();
    }

    public void Spawn()
    {
        //transform.position = spawn.position;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Carry(PickUp item)
    {
        Debug.Log("Picked up: " + item.name);
        isCarrying = true;
        carriedItem = item;
        item.StopGlow();
    }

    public void Drop(PickUp item)
    {
        Debug.Log("Dropped: " + item.name);
        isCarrying = false;
        carriedItem = null;
    }

    public void Throw(PickUp item)
    {
        Debug.Log("Threw: " + item.name);
        Drop(item);
        item.gameObject.GetComponent<Rigidbody>().AddForce(lookRay.direction*throwForce, ForceMode.Impulse);
    }

    private void Update()
    {
        lookRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(lookRay.origin, lookRay.direction * grabDistance, Color.red, 10, depthTest: false);
        Physics.Raycast(lookRay.origin, lookRay.direction, out hitData, grabDistance);
        try
        {
            if (!isCarrying && hitData.collider.tag == "Item")
            {
                lastSeenPickup = hitData.transform.gameObject.GetComponent<PickUp>();

                if (!lastSeenPickup.glowing)
                    lastSeenPickup.StartGlow();
            }
            else if (lastSeenPickup != null)
            {
                lastSeenPickup.StopGlow();
                lastSeenPickup = null;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isCarrying)
                {
                    Drop(carriedItem);
                }
                else
                {
                    if (hitData.collider.tag == "Item")
                    {
                        Carry(hitData.transform.gameObject.GetComponent<PickUp>());
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (isCarrying)
                {
                    Throw(carriedItem);
                }
            }
            else if (isCarrying)
            {
                objectPosition = Camera.main.transform.position + Camera.main.transform.forward * floatOffset;
                carriedItem.gameObject.transform.position = objectPosition;
                if (carriedItem.GetComponent<Rigidbody>().mass >= 10)
                {
                    firstPersonController.MoveSpeed = slowedMoveSpeed;
                }
            }
            //I could probably do this better with events
            else
            {
                firstPersonController.MoveSpeed = regMoveSpeed;
            }
        }
        catch (NullReferenceException n){

        }
    }
}
