using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class Mutant : EnemyAgent
{
    Animator animator;
    public GameObject model;
    // Start is called before the first frame update
    void Start()
    {
        animator = model.GetComponent<Animator>();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //Vector3 move = new Vector3(actions.ContinuousActions[0], actions.ContinuousActions[1], actions.ContinuousActions[2]);
        //animator.SetFloat("Speed", actions.ContinuousActions[0]);
        //animator.SetFloat("Turn", actions.ContinuousActions[1]);
    }
}
