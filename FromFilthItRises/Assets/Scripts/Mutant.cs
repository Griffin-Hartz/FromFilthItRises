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
        localSpeed = actions.ContinuousActions[0];
        localTurn = actions.ContinuousActions[1];
        //My plan is to have a separate, non-ml script on the mutant that will handle things like sounds, the attack animation, etc 
        //that would be really hard to train the agent to do
    }

    //I wish I could make it reset on player kill

    private void FixedUpdate()
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", localSpeed);
            animator.SetFloat("Turn", localTurn);
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (trainingMode && collision.collider.CompareTag("Boundary"))// 
        {
            // Collided with the area boundary, give a negative reward
            Debug.Log("Collide with boundary");
            AddReward(-.2f);
        }
        if (trainingMode && collision.collider.CompareTag("Building"))// 
        {
            // Collided with the area boundary, give a negative reward
            Debug.Log("Collide with building");
            AddReward(-.1f);
        }
        if (trainingMode && collision.collider.CompareTag("Player") && nearestPlayer.isAlive)// && validRun
        {
            nearestPlayer.Kill();
            AddReward(10);
            Debug.Log("Kill player");
            OnEpisodeBegin();
            animator.SetTrigger("Attack");
        }
    }

    //should add an onepisodebegin clause so that it doesn't spawn on top of player maybe
}
