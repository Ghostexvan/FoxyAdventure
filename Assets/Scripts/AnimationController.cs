using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{

    #region Declares

    MovementController movement;
    Animator animaton;
    Transform t;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        #region Get Component

        movement = GetComponent<MovementController>();
        animaton = GetComponent<Animator>();
        ValidateComponent();

        t = transform;

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        Flipping();

        if (movement.isDead){
            animaton.SetTrigger("Hurt");
        }

        if(movement.isGrounded){
            if(movement.isJumping)
                animaton.SetTrigger("Jump");
            else if (movement.isCrouching)
                animaton.SetTrigger("Crouch");
            else if (movement.isRunning)
                animaton.SetTrigger("Run");
            else
                animaton.SetTrigger("Idle");
        }
        else {
            if(movement.isJumping)
                animaton.SetTrigger("Jump");
            else
                animaton.SetTrigger("Fall");
        }
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
