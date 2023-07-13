using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBat : MonoBehaviour
{
    BatMovementController movement;
    public GameObject warning;
    public bool isWarning;
    public float timeWarning;

    void Awake(){
        warning.SetActive(false);
    }

    void Start(){
        movement = this.transform.parent.gameObject.GetComponent<BatMovementController>();
    }

    void Update(){
        if (isWarning)
            warning.SetActive(true);
        else
        {
            warning.SetActive(false);
        }
    }

    void OnTriggerStay2D(Collider2D col){
        //Debug.Log(col.gameObject.name + " - " + this.transform.parent.name);
        if (col.gameObject.name.Equals("Player")){
            //Debug.Log(col.gameObject.transform.position.x);
            if (!movement.isAttacking && !isWarning){
                timeWarning = Time.realtimeSinceStartup;
                isWarning = true;
                movement.targetPostionX = col.gameObject.transform.position.x;
                return;
            }
            else if (Time.realtimeSinceStartup - timeWarning >= 0.7f){
                movement.isAttacking = true;
                isWarning = false;
            }

            if (movement.isAttacking){
                isWarning = false;
                movement.time = Time.realtimeSinceStartup;
            }
        }
    }

    void OnTriggerExit2D(Collider2D col){
        if(!col.gameObject.name.Equals("Player") || !col.gameObject.GetComponent<Testing>().isCrouching )
            isWarning = false;
    }
}
