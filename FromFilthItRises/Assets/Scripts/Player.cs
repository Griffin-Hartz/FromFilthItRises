using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform spawn;
    private RaycastHit hitData;

    public void Spawn()
    {
        //transform.position = spawn.position;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Carry(PickUp item)
    {
        Debug.Log("Picked up: " + item.name);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Grabbing");
            //Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 10);
            Physics.Raycast(ray, out hitData);
            if(hitData.collider.tag == "Item")
            {
                Carry(hitData.transform.gameObject.GetComponent<PickUp>());
            }
        }
    }
}
