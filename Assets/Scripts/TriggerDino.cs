using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDino : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col){
        Debug.Log(col.gameObject.name + " - " + this.transform.parent.name);
        if (col.gameObject.name.Equals(this.transform.parent.name)){
            //Debug.Log("Hit");
            Dino_MovementController controller = col.gameObject.GetComponent<Dino_MovementController>();
            controller.time = Time.realtimeSinceStartup;
            controller.isWaitting = true;
        }
    }
}
