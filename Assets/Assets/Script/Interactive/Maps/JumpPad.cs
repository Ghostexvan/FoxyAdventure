using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpForce;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag != "Player")
            return;

        col.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        col.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, this.jumpForce);
    }
}
