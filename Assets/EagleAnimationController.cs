using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EagleMovementController))]
[RequireComponent(typeof(Animator))]
public class EagleAnimationController : MonoBehaviour
{
    #region Declares

    EagleMovementController movement;
    Animator animaton;
    Transform t;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        #region Get Component

        movement = GetComponent<EagleMovementController>();
        animaton = GetComponent<Animator>();
        ValidateComponent();

        t = transform;

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        Flipping();

        if(movement.isAttacking)
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
