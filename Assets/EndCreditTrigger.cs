using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCreditTrigger : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D col){
        Debug.Log(col.gameObject.transform.position.y + " - " + this.transform.parent.gameObject.transform.position.y);
        // if (col.gameObject.name.Equals("EndCredit") && col.gameObject.transform.position.y == this.gameObject.transform.position.y){
        //     if (isMoving){
        //         isMoving = false;
        //         time = Time.realtimeSinceStartup;
        //         return;
        //     }

        //     if (Time.realtimeSinceStartup - time >= 5.0f){
        //         SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        //     }
        // }
    }
}
