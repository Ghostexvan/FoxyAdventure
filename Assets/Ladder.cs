using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public GameObject Player;
    Testing movement;
    Animator animation;
    public bool climable;
    public float climbSpeed = 2.0f;

    void Awake(){
        if (!Player)
            return;
        
        movement = Player.GetComponent<Testing>();
        animation = Player.GetComponent<Animator>();
        climable = false;
    }

    void FixedUpdate(){
        if (!Player)
            return;

        if (!climable){
            movement.isClimbing = false;
            animation.speed = 1.0f;
            return;
        }

        if ((climable && Input.GetAxis("Vertical") > 0) || (climable && !movement.isGrounded)){
            movement.isClimbing = true;
        }
        else if (movement.isClimbing && movement.isGrounded && Input.GetAxis("Vertical") < 0){
            movement.isClimbing = false;
            animation.speed = 1.0f;
            return;
        }

        if (movement.isClimbing){
            movement.r2d.isKinematic = true;
        }
        else
            movement.r2d.isKinematic = false;
            
        Move();
    }

    void OnTriggerStay2D(Collider2D col){
        if (!Player)
            return;

        if (col.gameObject.name.Equals("Player")){
            Debug.Log("Enter");
            climable = true;
        }
    }

    void OnTriggerExit2D(Collider2D col){
        if (!Player)
            return;

        if (col.gameObject.name.Equals("Player")){
            Debug.Log("Exit");
            climable = false;
        }
    }

    void Move(){
        if(!movement.isClimbing)
            return;

        movement.r2d.velocity = new Vector2(0.0f, 0.0f);

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        float amntToMove1 = climbSpeed * Time.deltaTime;

        if (hor > 0){
            Player.transform.Translate(Vector2.right * amntToMove1);
            animation.speed = 0.3f;
        }
        else if (hor < 0){
            Player.transform.Translate(Vector2.left * amntToMove1);
            animation.speed = 0.3f;
        }
            
        if (ver > 0){
            Player.transform.Translate(Vector2.up * amntToMove1);
            animation.speed = 0.3f;
        }
            
        else if (ver < 0){
            Player.transform.Translate(Vector2.down * amntToMove1);
            animation.speed = 0.3f;
        }
            
        if (hor == 0 && ver == 0)
            animation.speed = 0.0f;
    }
}
