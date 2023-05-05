using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform spawn;
    [SerializeField] private Transform inFrontOfPlayer;
    [SerializeField] private float throwForce = 1f;
    [SerializeField] private float floatOffset = 1f;
    [SerializeField] private float grabDistance = 1f;
    [SerializeField] private bool isCarrying = false;
    public Vector3 objectPosition;
    private Ray lookRay;
    private RaycastHit hitData;
    private PickUp carriedItem;
    private PickUp lastSeenPickup; 
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
        //Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        lookRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(lookRay.origin, lookRay.direction * grabDistance);
        Physics.Raycast(lookRay, out hitData);
        if (!isCarrying && hitData.collider.tag == "Item")
        {
            lastSeenPickup = hitData.transform.gameObject.GetComponent<PickUp>();             
            
            if(!lastSeenPickup.glowing)
                lastSeenPickup.StartGlow();
        }
        else if(lastSeenPickup != null)
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
            //make this better so it moves around in front of them
            objectPosition = Camera.main.transform.position + Camera.main.transform.forward*floatOffset;
            //objectPosition.y = carryHeightOffset;
            carriedItem.gameObject.transform.position = objectPosition;
        }
    }
}
