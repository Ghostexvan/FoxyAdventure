using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BettleController : DogController1
{
    new void Awake()
    {
        this._physics.isKinematic = true;
        base.Awake();
    }

    new void Update()
    {

    }

    new public void FixedUpdate()
    {
        base.FixedUpdate();
        this.HandleMove();
    }

    new void HandleMove()
    {
        this.move();
        this.isMoving = true;
        this.moveDirection = this.isFacingRight ? 1 : -1;
    }
}
