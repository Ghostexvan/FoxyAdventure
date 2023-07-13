using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Dino_MovementController))]
[RequireComponent(typeof(Animator))]
public class Dino_AnimationController : MonoBehaviour
{
    #region Declares

    Dino_MovementController movement;
    Animator animaton;
    Transform t;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        #region Get Component

        movement = GetComponent<Dino_MovementController>();
        animaton = GetComponent<Animator>();
        ValidateComponent();

        t = transform;

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        Flipping();

        if (!movement.isAlive)
            animaton.SetTrigger("Destroy");

        if(movement.isRunning)
            animaton.SetTrigger("Run");
        else
            animaton.SetTrigger("Idle");
    }

    void ValidateComponent(){
        if (movement == null || animaton == null){
            Debug.Log("Component missing!");
            Application.Quit();
        }
    }

    void Flipping(){
        if(movement.isFacingRight)
            t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
        else
            t.localScale = new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
    }
}
