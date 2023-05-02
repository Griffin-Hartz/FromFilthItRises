using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform spawn;
    
    public void Spawn()
    {
        Debug.Log("Spawn");
        transform.position = spawn.position;
    }
}
