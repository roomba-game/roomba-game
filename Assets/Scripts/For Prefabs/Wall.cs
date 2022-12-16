using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : BaseGridObject
{
    // makes the player robot come to a halt when it collides with a wall
    public override bool isSolid { get { return true; } }
}
