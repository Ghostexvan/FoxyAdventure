using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Testing))]
[RequireComponent(typeof(Animator))]
public class TestingAnimation : MonoBehaviour
{

    #region Declares

    Testing movement;
    Animator animaton;
    Transform t;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        #region Get Component

        movement = GetComponent<Testing>();
        animaton = GetComponent<Animator>();
        ValidateComponent();

        t = transform;

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        Flipping();

        if (movement.isHurt){
            animaton.SetTrigger("Hurt");
            goto SkipToEnd;
        }

        if (movement.isClimbing){
            animaton.SetTrigger("Climb");
            goto SkipToEnd;
        }
        
        if(movement.isJumping){
            animaton.SetTrigger("Jump");
            goto SkipToEnd;
        }

        if (movement.isFalling){
            animaton.SetTrigger("Fall");
            goto SkipToEnd;
        }
            
        if (movement.isCrouching){
            animaton.SetTrigger("Crouch");
            goto SkipToEnd;
        }
            
        if (movement.isRunning){
            animaton.SetTrigger("Run");
            goto SkipToEnd;
        }
            
        if(movement.isGrounded){
            animaton.SetTrigger("Idle");
        }

        SkipToEnd:
            return;
    }

    void ValidateComponent(){
        if (movement == null || animaton == null){
            Debug.Log("Component missing!");
            Application.Quit();
        }
    }

    void Flipping(){
        if(movement.isFacingRight)
            t.localScale = new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
        else
            t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
    }
}
