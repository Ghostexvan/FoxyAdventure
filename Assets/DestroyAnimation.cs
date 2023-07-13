using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAnimation : MonoBehaviour
{
    public GameObject enemies;
    Animation _animation;

    void Awake(){
        _animation = GetComponent<Animation>();
    }
    
    void FixedUpdate(){
        if (!enemies){
            _animation.Play();
            Destroy(this.gameObject, 0.5f);
        }
    }
}
