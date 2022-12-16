using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : BaseGridObject
{
  // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    // makes the player robot pass through the mess
    public override bool isSolid { get { return false; } }

    public override void OnCollision() { }
}
