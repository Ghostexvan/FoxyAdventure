using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoController1 : DogController1
{
    public Collider2D trigger;
    public float multiply;
    public bool isTrigger;

    new void FixedUpdate()
    {
        if (this.isTrigger)
        {
            this.CheckOnGround();
            this.move();
        }
        else
        {
            base.FixedUpdate();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            this.moveSpeed = this.maxSpeed * this.multiply;
            this.minSpeed = this.moveSpeed;
            this.isTrigger = true;
        }
    }

//    void SetAnimation()
//    {
//        if (!this.isOnGround)
//            this._ani.SetTrigger("Idle");
//        else
//            this._ani.SetTrigger("Run");
//    }
}
