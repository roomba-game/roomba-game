using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : BaseGridObject
{
    public Rigidbody[] phisicsParts;

    // makes the player robot pass through the obstacle
    public override bool isSolid { get { return false; } }

    // obstacle explodes over when it makes contact with player 
    public override void OnCollision()
    {
        foreach (Rigidbody phisicsPart in phisicsParts)
        {
            phisicsPart.isKinematic = false;
        }
        Player.P.LockMovement();
        SFX.S.Play(2);
        StartCoroutine(Main.S.endLevel(false, 4));
    }
}
