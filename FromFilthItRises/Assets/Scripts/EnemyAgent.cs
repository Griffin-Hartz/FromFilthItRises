using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// A hummingbird Machine Learning Agent
/// </summary>
public class EnemyAgent : Agent
{
    [Tooltip("Force to apply when moving")]
    public float moveForce = 2f;

    [Tooltip("Speed to pitch up or down")]
    public float pitchSpeed = 100f;

    [Tooltip("Speed to rotate around the up axis")]
    public float yawSpeed = 100f;

    [Tooltip("Transform of attacking appendage")]
    public Transform claw;

    [Tooltip("The agent's camera")]
    public Camera agentCamera;

    [Tooltip("Whether this is training mode or gameplay mode")]
    public bool trainingMode;

    [Tooltip("The color when the run is valid")]
    public Color validColor;

    [Tooltip("The color when the run is invalid")]
    public Color invalidColor;

    [SerializeField] private Material enemyMaterial;

    //Disables run if spawns on top of player
    private bool validRun = true;

    // The rigidbody of the agent
    new private Rigidbody rigidbody;

    // The nearest player to the agent
    [SerializeField] private PlayerAgent nearestPlayer;

    // Allows for smoother pitch changes
    private float smoothPitchChange = 0f;

    // Allows for smoother yaw changes
    private float smoothYawChange = 0f;

    // Maximum angle that the bird can pitch up or down
    private const float MaxPitchAngle = 80f;

    // Maximum distance from the claw to accept target collision
    private const float BeakTipRadius = 0.008f;

    // Whether the agent is frozen (intentionally not flying)
    private bool frozen = false;

    EnvironmentParameters defaultParameters;

    /// <summary>
    /// The amount of target the agent has obtained this episode
    /// </summary>
    public float NectarObtained { get; private set; }

    /// <summary>
    /// Initialize the agent
    /// </summary>
    public override void Initialize()
    {
        rigidbody = GetComponent<Rigidbody>();
        //playerArea = GetComponentInParent<PlayerArea>();

        // If not training mode, no max step, play forever
        if (!trainingMode) MaxStep = 0;

        defaultParameters = Academy.Instance.EnvironmentParameters;
    }

    /// <summary>
    /// Reset the agent when an episode begins
    /// </summary>
    public override void OnEpisodeBegin()
    {
        if (trainingMode)
            nearestPlayer.ResetPlayer();

        // Reset player health
        // playerHealth = 0f;
        Debug.Log("resetting player");
        // Zero out velocities so that movement stops before a new episode begins
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        // Default to spawning in front of a player
        bool inFrontOfPlayer = true;
        if (trainingMode)
        {
            // Spawn in front of player 50% of the time during training
            inFrontOfPlayer = UnityEngine.Random.value > .5f;
        }
        validRun = true;
        // Move the agent to a new random position
        MoveToSafeRandomPosition(inFrontOfPlayer);
        Debug.Log("valid: " + validRun);
        if (validRun)
        {
            enemyMaterial.SetColor("_BaseColor", validColor);
        }
        else
        {
            enemyMaterial.SetColor("_BaseColor", invalidColor);
        }
    }

    /// <summary>
    /// So the lowdown on this method is that actionbuffers are essentially choices that the agent can make
    /// 
    /// </summary>
    /// <param name="actions"></param>
    public override void OnActionReceived(ActionBuffers actions)
    {
        //Debug.Log("Actionrecieved: " + actions.ToString());

        //Vector3 move = new Vector3(actions.DiscreteActions[0], 0,0);
        Vector3 move = new Vector3(actions.ContinuousActions[0], actions.ContinuousActions[1], actions.ContinuousActions[2]);

        //Debug.Log(actions.ContinuousActions[0] + "continuous");


        //Vector3 move = new Vector3(actions.DiscreteActions[0], actions.DiscreteActions[1], actions.DiscreteActions[2]);

        // Add force in the direction of the move vector
        rigidbody.AddForce(move * moveForce);
        //transform.Translate(move * moveForce);

        /*
        // Convert the first action to forward movement
        float forwardAmount = actions.DiscreteActions[0];

        // Convert the second action to turning left or right
        float turnAmount = 0f;

        //gives array out of bounds error?
        if (actions.DiscreteActions[1] == 1f)
        {
            turnAmount = -1f;
        }
        else if (actions.DiscreteActions[1] == 2f)
        {
            turnAmount = 1f;
        }

        int moveSpeed = 1;
        int turnSpeed = 1;

        // Apply movement
        rigidbody.MovePosition(transform.position + transform.forward * forwardAmount * moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(transform.up * turnAmount * turnSpeed * Time.fixedDeltaTime);

        // Apply a tiny negative reward every step to encourage action
        if (MaxStep > 0) AddReward(-1f / MaxStep);*/
    }

    /// <summary>
    /// Collect vector observations from the environment
    /// </summary>
    /// <param name="sensor">The vector sensor</param>
    public override void CollectObservations(VectorSensor sensor)
    {
        //Debug.Log("Collecting observations");

        // If nearestPlayer is null, observe an empty array and return early
        if (nearestPlayer == null)
        {
            Debug.Log("return early observation, player null");
            sensor.AddObservation(new float[10]);
            return;
        }

        // Observe the agent's local rotation (4 observations)
        sensor.AddObservation(transform.localRotation.normalized);

        // Get a vector from the claw to the nearest player
        Vector3 toPlayer = nearestPlayer.PlayerCenterPosition - claw.position;
        //Debug.Log("To player: " + toPlayer);

        // Observe a normalized vector pointing to the nearest player (3 observations)
        sensor.AddObservation(toPlayer.normalized);

        // Observe a dot product that indicates whether the claw is in front of the player (1 observation)
        // (+1 means that the claw is directly in front of the player, -1 means directly behind)
        sensor.AddObservation(Vector3.Dot(toPlayer.normalized, -nearestPlayer.PlayerUpVector.normalized));

        // Observe a dot product that indicates whether the beak is pointing toward the player (1 observation)
        // (+1 means that the beak is pointing directly at the player, -1 means directly away)
        sensor.AddObservation(Vector3.Dot(claw.forward.normalized, -nearestPlayer.PlayerUpVector.normalized));

        // Observe the relative distance from the claw to the player (1 observation)
        sensor.AddObservation(toPlayer.magnitude);

        // 10 total observations
    }

    /// <summary>
    /// When Behavior Type is set to "Heuristic Only" on the agent's Behavior Parameters,
    /// this function will be called. Its return values will be fed into
    /// <see cref="OnActionReceived(float[])"/> instead of using the neural network
    /// </summary>
    /// <param name="actionsOut">And output action array</param>
    public void Heuristic(float[] actionsOut)
    {
        // Create placeholders for all movement/turning
        Vector3 forward = Vector3.zero;
        Vector3 left = Vector3.zero;
        Vector3 up = Vector3.zero;
        float pitch = 0f;
        float yaw = 0f;

        // Convert keyboard inputs to movement and turning
        // All values should be between -1 and +1

        // Forward/backward
        if (Input.GetKey(KeyCode.W)) forward = transform.forward;
        else if (Input.GetKey(KeyCode.S)) forward = -transform.forward;

        // Left/right
        if (Input.GetKey(KeyCode.A)) left = -transform.right;
        else if (Input.GetKey(KeyCode.D)) left = transform.right;

        // Up/down
        if (Input.GetKey(KeyCode.E)) up = transform.up;
        else if (Input.GetKey(KeyCode.C)) up = -transform.up;

        // Pitch up/down
        if (Input.GetKey(KeyCode.UpArrow)) pitch = 1f;
        else if (Input.GetKey(KeyCode.DownArrow)) pitch = -1f;

        // Turn left/right
        if (Input.GetKey(KeyCode.LeftArrow)) yaw = -1f;
        else if (Input.GetKey(KeyCode.RightArrow)) yaw = 1f;

        // Combine the movement vectors and normalize
        Vector3 combined = (forward + left + up).normalized;

        // Add the 3 movement values, pitch, and yaw to the actionsOut array
        actionsOut[0] = combined.x;
        actionsOut[1] = combined.y;
        actionsOut[2] = combined.z;
        actionsOut[3] = pitch;
        actionsOut[4] = yaw;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);
    }

    /// <summary>
    /// Prevent the agent from moving and taking actions
    /// </summary>
    public void FreezeAgent()
    {
        Debug.Assert(trainingMode == false, "Freeze/Unfreeze not supported in training");
        frozen = true;
        rigidbody.Sleep();
    }

    /// <summary>
    /// Resume agent movement and actions
    /// </summary>
    public void UnfreezeAgent()
    {
        Debug.Assert(trainingMode == false, "Freeze/Unfreeze not supported in training");
        frozen = false;
        rigidbody.WakeUp();
    }

    /// <summary>
    /// Move the agent to a safe random position (i.e. does not collide with anything)
    /// If in front of player, also point the beak at the player
    /// </summary>
    /// <param name="inFrontOfPlayer">Whether to choose a spot in front of a player</param>
    private void MoveToSafeRandomPosition(bool inFrontOfPlayer)
    {
        Debug.Log("Moving to a safe random position. Infrontofplayer: " + inFrontOfPlayer);
        bool safePositionFound = false;
        int attemptsRemaining = 100; // Prevent an infinite loop
        Vector3 potentialPosition = Vector3.zero;
        Quaternion potentialRotation = new Quaternion();

        // Loop until a safe position is found or we run out of attempts
        while (!safePositionFound && attemptsRemaining > 0)
        {
            attemptsRemaining--;
            if (inFrontOfPlayer)
            {
                // Position 10 to 20 cm in front of the player
                float distanceFromPlayer = UnityEngine.Random.Range(1f, 2f);
                potentialPosition = nearestPlayer.transform.position + nearestPlayer.PlayerUpVector * distanceFromPlayer;

                // Point beak at player (bird's head is center of transform)
                Vector3 toPlayer = nearestPlayer.PlayerCenterPosition - potentialPosition;
                potentialRotation = Quaternion.LookRotation(toPlayer, Vector3.up);
            }
            else
            {
                // Pick a random height from the ground
                float height = UnityEngine.Random.Range(1.2f, 2.5f);

                // Pick a random radius from the center of the area
                float radius = UnityEngine.Random.Range(2f, 7f);

                // Pick a random direction rotated around the y axis
                Quaternion direction = Quaternion.Euler(0f, UnityEngine.Random.Range(-180f, 180f), 0f);

                // Combine height, radius, and direction to pick a potential position
                potentialPosition = nearestPlayer.transform.position + Vector3.up * height + direction * Vector3.forward * radius;

                // Choose and set random starting pitch and yaw
                float pitch = UnityEngine.Random.Range(-60f, 60f);
                float yaw = UnityEngine.Random.Range(-180f, 180f);
                potentialRotation = Quaternion.Euler(pitch, yaw, 0f);
            }

            // Check to see if the agent will collide with anything
            Collider[] colliders = Physics.OverlapSphere(potentialPosition, 0.05f);

            // Safe position has been found if no colliders are overlapped
            safePositionFound = colliders.Length == 0;
        }

        Debug.Assert(safePositionFound, "Could not find a safe position to spawn");
        validRun = safePositionFound;
        // Set the position and rotation
        transform.position = potentialPosition;
        transform.rotation = potentialRotation;
    }

    /// <summary>
    /// Called when the agent's collider enters a trigger collider
    /// </summary>
    /// <param name="other">The trigger collider</param>
    /*private void OnTriggerEnter(Collider other)
    {
        TriggerEnterOrStay(other);
    }

    /// <summary>
    /// Called when the agent's collider stays in a trigger collider
    /// </summary>
    /// <param name="other">The trigger collider</param>
    /private void OnTriggerStay(Collider other)
    {
        TriggerEnterOrStay(other);
    }*/

    /// <summary>
    /// Handles when the agen'ts collider enters or stays in a trigger collider
    /// </summary>
    /// <param name="collider">The trigger collider</param>
    /*private void TriggerEnterOrStay(Collider collider)
    {
        Debug.Log("Collision with " + collider.tag);
        // Check if agent is colliding with target
        if (collider.CompareTag("Player"))
        {
            Vector3 closestPointToClaw = collider.ClosestPoint(claw.position);

            // Check if the closest collision point is close to the claw
            // Note: a collision with anything but the claw should not count
            if (Vector3.Distance(claw.position, closestPointToClaw) < BeakTipRadius)
            {
                // Look up the player for this target collider
                //PlayerAgent player = playerArea.GetPlayerFromNectar(collider);

                // Attempt to take .01 target
                // Note: this is per fixed timestep, meaning it happens every .02 seconds, or 50x per second
                nearestPlayer.Kill();

                if (trainingMode)
                {
                    // Calculate reward for getting target
                    float bonus = .02f * Mathf.Clamp01(Vector3.Dot(transform.forward.normalized, -nearestPlayer.PlayerUpVector.normalized));
                    AddReward(.01f + bonus);
                }
            }
        }
    }*/

    /// <summary>
    /// Called when the agent collides with something solid
    /// </summary>
    /// <param name="collision">The collision info</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (trainingMode && collision.collider.CompareTag("Boundary") && validRun)// 
        {
            // Collided with the area boundary, give a negative reward
            Debug.Log("Collide with boundary");
            AddReward(-.2f);
        }
        if (trainingMode && collision.collider.CompareTag("Building") && validRun)// 
        {
            // Collided with the area boundary, give a negative reward
            Debug.Log("Collide with building");
            AddReward(-.1f);
        }
        if (trainingMode && collision.collider.CompareTag("Player") && validRun && nearestPlayer.isAlive)// && validRun
        {
            nearestPlayer.Kill();
            AddReward(10);
            Debug.Log("Kill player");
        }
    }

    /// <summary>
    /// Called every frame
    /// </summary>
    private void Update()
    {
        // Draw a line from the claw to the nearest player
        if (nearestPlayer != null)
            Debug.DrawLine(claw.position, nearestPlayer.PlayerCenterPosition, Color.green);
    }
}
