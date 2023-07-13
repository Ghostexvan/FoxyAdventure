using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleFindPlayer : MonoBehaviour
{
    EagleMovementController movement;
    void Start(){
        movement = this.transform.parent.gameObject.GetComponent<EagleMovementController>();
    }

    void OnTriggerStay2D(Collider2D col){
        //Debug.Log(col.gameObject.name + " - " + this.transform.parent.name);
        if (col.gameObject.name.Equals("Player")){
            if (this.transform.parent.gameObject.transform.position.x - col.gameObject.transform.position.x >= 0)
                movement.isFacingRight = false;
            else 
                movement.isFacingRight = true;
        }
    }
}
