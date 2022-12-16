using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class provides a base for objects in the game grid such as plants and walls.
/// </summary>
public abstract class BaseGridObject : MonoBehaviour
{
    /// <summary>
    /// true is the robot should stop when it collides with this object
    /// false if the robot should pass through this object
    /// </summary>
    public abstract bool isSolid { get; }

    /// <summary>
    /// This method is called when the player robot collides with this object
    /// </summary>
    public virtual void OnCollision() { }
}
