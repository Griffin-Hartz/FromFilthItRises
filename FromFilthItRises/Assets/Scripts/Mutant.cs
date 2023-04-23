using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System;

public class Mutant : EnemyAgent
{
    Animator animator;
    float localSpeed = 0;
    float localTurn = 0;
    bool attacking = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //Vector3 move = new Vector3(actions.ContinuousActions[0], actions.ContinuousActions[1], actions.ContinuousActions[2]);
        localSpeed = actions.ContinuousActions[0];
        localTurn = actions.ContinuousActions[1];
        //animator.
        if (actions.ContinuousActions[2] > 0.99)
        {
            attacking = true;
        }
        else
        {
            attacking = false;
        }
        Debug.Log("Test Speed: " + Math.Round(actions.ContinuousActions[0], 3));
    }

    private void FixedUpdate()
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", localSpeed);
            animator.SetFloat("Turn", localTurn);
        }
        if (attacking)
        {
            animator.SetTrigger("Attack");
        }
    }

    //should add an onepisodebegin clause so that it doesn't spawn on top of player maybe
}
