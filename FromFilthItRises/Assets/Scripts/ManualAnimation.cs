using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualAnimation : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("Walk");
            anim.SetFloat("Speed", 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            anim.SetFloat("Speed", -1);
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            anim.SetFloat("Turn", -1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            anim.SetFloat("Turn", 1);
        }
        else
        {
            anim.SetFloat("Turn", 0);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            anim.SetTrigger("Attack");
        }
    }
}
