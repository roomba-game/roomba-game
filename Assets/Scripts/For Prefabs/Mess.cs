using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mess : BaseGridObject
{ // Inherit from abstract collision class
  // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    // makes the player robot pass through the mess
    public override bool isSolid { get { return false; } }

    public override void OnCollision()
    {
        // Tell the Main singleton that this mess has been destroyed
        Main.S.MessDestroyed();

        // Destroy this Mess
        Destroy(this.gameObject);
    }
}
