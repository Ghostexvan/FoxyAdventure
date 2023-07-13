using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.SendMessage("AddDamage", 1);
            Destroy(this.gameObject);
        }
            
        else
            Destroy(this.gameObject);
    }
}
