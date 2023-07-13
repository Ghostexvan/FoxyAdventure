using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class BatMovementController : MonoBehaviour
{
    #region Declares

    public bool isGrounded,
                isFacingRight,
                isRunning,
                isWaitting,
                isHit,
                isAttacking;

    public double time;

    [SerializeField] private LayerMask triggerLayer;

    private BoxCollider2D mainCollider;
    float moveDirection;
    public float targetPostionX;
    public float maxSpeed = 1f;
    public float gravityScale = 1.5f;
    Vector2 left = Vector2.left;
    Vector2 right = Vector2.right;

    Transform t;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        #region Get Component

        mainCollider = GetComponent<BoxCollider2D>();

        if(!ValidateComponent()){
            Debug.Log("Component missing!");
            Application.Quit();
        }
        
        #endregion

        #region Start state

        t = transform;

        isRunning = isWaitting = isAttacking = false;
        
        isFacingRight = false;
        time = 0.0f;

        #endregion
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate(){
        CheckAttack();
    }

    bool ValidateComponent(){
        if (mainCollider == null)
            return false;
        return true;
    }

    void CheckAttack(){
        if (!isAttacking)
            return;

        moveDirection = this.transform.position.x - targetPostionX;
        //Debug.Log(moveDirection + " - " + isAttacking);
        if (moveDirection < 0.05 && moveDirection > -0.05)
            isAttacking = false;
        else if (moveDirection < 0){
            isFacingRight = true;
            float amntToMove1 = maxSpeed * Time.deltaTime;
            this.transform.Translate(Vector2.right * amntToMove1);
        }
        else{
            isFacingRight = false;
            float amntToMove1 = maxSpeed * Time.deltaTime;
            this.transform.Translate(Vector2.left * amntToMove1);
        }
        
    }

    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag == "death_collider"){
            Destroy(this.gameObject);
        }
    }
}
