using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System;

public class Mutant : EnemyAgent
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //Vector3 move = new Vector3(actions.ContinuousActions[0], actions.ContinuousActions[1], actions.ContinuousActions[2]);
        animator.SetFloat("Speed", actions.ContinuousActions[0]);
        animator.SetFloat("Turn", actions.ContinuousActions[1]);
        if (actions.ContinuousActions[2] > 0.99)
        {
            animator.SetTrigger("Attack");
        }
        Debug.Log("Test Speed: " + Math.Round(actions.ContinuousActions[0], 3));
    }

    //should add an onepisodebegin clause so that it doesn't spawn on top of player maybe
}
