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
    public Color fullPlayerColor = new Color(1f, 0f, .3f);

    [Tooltip("The color when the player is empty")]
    public Color emptyPlayerColor = new Color(.5f, 0f, 1f);

    // The solid collider representing the player
    [SerializeField] private Collider playerCollider;

    // The player's material
    [SerializeField] private Material playerMaterial;

    //Is the player alive
    private bool isAlive;

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
    /// Resets the player
    /// </summary>
    public void ResetPlayer()
    {
        isAlive= true;

        // Enable the player collider
        playerCollider.gameObject.SetActive(true);

        // Change the player color to indicate that it is full
        playerMaterial.SetColor("_BaseColor", fullPlayerColor);
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
    }
}
