using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public GameObject gameCheck;
    public GameObject player;

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.name.Equals("Player")){
            gameCheck.GetComponent<GameCheck>().ChangePosition(this.gameObject.transform.position);
            player.GetComponent<Testing>().isChecked = true;
            player.GetComponent<Testing>().checkTime = Time.realtimeSinceStartup;
            this.gameObject.SetActive(false);
        }
    }
}
