using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != "Player")
            return;

        col.gameObject.SendMessage("SetSpawnpoint", (Vector2)this.transform.position);
        Destroy(this.gameObject);
    }
}
