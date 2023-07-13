using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideLayer : MonoBehaviour
{
    GameObject _object;

    void Start(){
        _object = GameObject.FindGameObjectWithTag(this.gameObject.name);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.name.Equals("Player")){
            _object.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Player") && !col.gameObject.GetComponent<Testing>().isCrouching){
            //Debug.Log("out");
            _object.SetActive(true);
        }
    }
}
