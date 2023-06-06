using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used as a dummy player for the purposes of training the mutant
/// </summary>
public class PlayerAgent : MonoBehaviour
{
    [Tooltip("The color when the player is full")]
    public Color fullPlayerColor;

    [Tooltip("The color when the player is empty")]
    public Color emptyPlayerColor;

    // The solid collider representing the player
    [SerializeField] private Collider playerCollider;

    // The player's material
    [SerializeField] private Material playerMaterial;

    //Is the player alive
    public bool isAlive;

    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private Transform spawnPositionsobj;

    /// <summary>
    /// A vector pointing straight out of the player
    /// </summary>
    public Vector3 PlayerUpVector
    {
        get
        {
            return playerCollider.transform.up;
        }
    }

    /// <summary>
    /// The center position of the nectar collider
    /// </summary>
    public Vector3 PlayerCenterPosition
    {
        get
        {
            return playerCollider.transform.position;
        }
    }

    /// <summary>
    /// The amount of nectar remaining in the player
    /// </summary>
    public float NectarAmount { get; private set; }

    /// <summary>
    /// Whether the player is alive
    /// </summary>
    public bool CheckAlive
    {
        get
        {
            return isAlive;
        }
    }

    /// <summary>
    /// Attempts to kill the player
    /// </summary>
    public void Kill()
    {
        playerMaterial.SetColor("_BaseColor", emptyPlayerColor);
        isAlive = false;
    }

    /// <summary>
    /// Move the agent to a safe random position (i.e. does not collide with anything)
    /// If in front of player, also point the beak at the player
    /// </summary>
    /// <param name="inFrontOfPlayer">Whether to choose a spot in front of a player</param>
    private void MoveToSafeRandomPosition(bool validRun)
    {
        Debug.Log("Moving player to a safe random position " + validRun);

        System.Random rnd = new System.Random();
        int spawnpoint = rnd.Next(0, spawnPositions.Length);
        transform.position = spawnPositions[spawnpoint].position;
        transform.rotation = spawnPositions[spawnpoint].rotation;
        /*bool safePositionFound = false;
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
        transform.rotation = potentialRotation;*/
    }

    /// <summary>
    /// Resets the player
    /// </summary>
    public void ResetPlayer()
    {
        isAlive= true;

        // Enable the player collider
        playerCollider.gameObject.SetActive(true);

        // Change the player color to indicate that it is full
        playerMaterial.SetColor("_BaseColor", fullPlayerColor);

        MoveToSafeRandomPosition(true);
    }



    /// <summary>
    /// Called when the player wakes up
    /// </summary>
    private void Awake()
    {
        // Find the player's mesh renderer and get the main material
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        if(playerMaterial == null)
            playerMaterial = meshRenderer.material;

        // Find player collider
        if(playerCollider == null)
            playerCollider = transform.Find("PlayerCollider").GetComponent<Collider>();

        spawnPositions = spawnPositionsobj.GetComponentsInChildren<Transform>();
    }
}
