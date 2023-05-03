using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform spawn;
    private RaycastHit hitData;
    private bool isCarrying = false;
    private PickUp carriedItem;
    private PickUp lastSeenPickup; 
    [SerializeField] private Transform inFrontOfPlayer;
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

    private void Update()
    {
        //Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10);
        Physics.Raycast(ray, out hitData);
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
        else if (isCarrying)
        {
            carriedItem.gameObject.transform.position = inFrontOfPlayer.position;
        }
    }
}
