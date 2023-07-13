using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBorder : MonoBehaviour
{
    GameObject _object;
    BatMovementController movement;
    float firstPositionX;

    void Awake(){
        _object = GameObject.Find(this.gameObject.name);
        
        if (!_object)
            return;

        Debug.Log(_object.gameObject.name);
        movement = _object.GetComponent<BatMovementController>();
        firstPositionX = _object.transform.position.x;
    }

    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.name.Equals(_object.gameObject.name)){
            movement.targetPostionX = firstPositionX;
        }
    }
}
