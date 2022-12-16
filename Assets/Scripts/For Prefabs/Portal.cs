using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : BaseGridObject
{

    // makes the player robot pass through the portal
    public override bool isSolid { get { return false; } }

    /// <summary>
    /// The part of the prefab to applie the color to.
    /// Can be used to change the color of the portal.
    /// </summary>
    public GameObject coloredPart;

    /// <summary>
    /// the coordinates of the other portal
    /// <example> <code>
    /// Main.S.grid[portal1.destination.z, portal1.destination.x]
    /// </code> </example>
    /// </summary>
    public (int x, int z) destination;

    // does portal things when player hits portal
    public override void OnCollision()
    {
        Player.P.exitDestination = destination;

        // TODO teleport player and play sound
        Debug.Log("teleport to " + destination);
    }
}
