using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleMovementController : MonoBehaviour
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
    public float targetPostionY;
    float PositionY;
    public float maxSpeed = 5.0f;
    public float gravityScale = 1.5f;

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

        PositionY = this.transform.position.y;

        #endregion
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate(){
        CheckAttack();
        Return();
    }

    bool ValidateComponent(){
        if (mainCollider == null)
            return false;
        return true;
    }

    void CheckAttack(){
        if (!isAttacking)
            return;

        moveDirection = this.transform.position.y - targetPostionY;
        Debug.Log(moveDirection + " - " + isAttacking);
        if (moveDirection <= 0){
            isAttacking = false;
            return;
        }
        float amntToMove1 = maxSpeed * Time.deltaTime;
        this.transform.Translate(Vector2.down * amntToMove1);
    }

    void Return(){
        if (!isAttacking && this.transform.position.y != PositionY){
            float amntToMove1 = maxSpeed * Time.deltaTime;
            this.transform.Translate(Vector2.up * amntToMove1);
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag == "death_collider"){
            Destroy(this.gameObject);
        }
    }
}
