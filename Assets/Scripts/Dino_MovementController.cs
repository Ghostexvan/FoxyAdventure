using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Dino_MovementController : MonoBehaviour
{
    #region Declares

    public bool isGrounded,
                isFacingRight,
                isRunning,
                isWaitting,
                isHit,
                isAttacking,
                isAlive;

    public double time;

    public GameObject warning, leftborder, rightborder;
    public bool isWarning;
    public float timeWarning;

    [SerializeField] private LayerMask triggerLayer, PlayerLayer;

    private BoxCollider2D mainCollider;
    float moveDirection;
    public float maxSpeed = 3.4f;
    public float gravityScale = 1.5f;
    Vector2 left = Vector2.left;
    Vector2 right = Vector2.right;

    Rigidbody2D r2d;
    Transform t;

    #endregion

    void Awake(){
        isAlive = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        #region Get Component

        mainCollider = GetComponent<BoxCollider2D>();
        r2d = GetComponent<Rigidbody2D>();

        if(!ValidateComponent()){
            Debug.Log("Component missing!");
            Application.Quit();
        }
        
        #endregion

        #region Start state

        t = transform;

        r2d.freezeRotation = true;
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        r2d.gravityScale = gravityScale;

        isRunning = isWaitting = isAttacking = false;
        
        isFacingRight = false;
        isWarning = false;
        isGrounded = onGround();

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
            return;

        if (isWarning){
            warning.SetActive(true);
            isWaitting = false;
        }
        else
        {
            warning.SetActive(false);
        }

        //Debug.Log("Run: " + isRunning + ", Wait: " + isWaitting + ", Facing Right: " + isFacingRight + ", Direction: " + moveDirection);
        moveDirection = isFacingRight ? 1 : -1;

        if(!isWaitting && isGrounded && !isWarning)
            isRunning = true;
        else
        {
            isRunning = false;
            moveDirection = 0;
        }

        if (isWaitting && Time.realtimeSinceStartup - time >= 2.0f){
            Flipping();
            isWaitting = false;
        }
    }

    void FixedUpdate(){
        if (!isAlive)
            return;

        CheckAbove();

        isGrounded = onGround();    

        if (isRunning){
            //DetectMovementRange();
            r2d.velocity = new Vector2((moveDirection) * maxSpeed, r2d.velocity.y);
        }
        
        DetectPlayer();
        Warning();    

    }

    void CheckAbove(){
        Vector3 bottom = new Vector3(mainCollider.bounds.min.x, mainCollider.bounds.max.y, 0f);
        Vector3 mid = new Vector3(mainCollider.bounds.center.x, mainCollider.bounds.max.y, 0f);
        Vector3 top = new Vector3(mainCollider.bounds.max.x, mainCollider.bounds.max.y, 0f);

        PlayerLayer = LayerMask.GetMask("Player");
        RaycastHit2D hit_top = Physics2D.CircleCast(top, mainCollider.size.x, Vector2.up, 0.45f, PlayerLayer);
        RaycastHit2D hit_mid = Physics2D.CircleCast(mid, mainCollider.size.x, Vector2.up, 0.45f, PlayerLayer);
        RaycastHit2D hit_bottom = Physics2D.CircleCast(bottom, mainCollider.size.x, Vector2.up, 0.45f, PlayerLayer);

        if (hit_top.collider != null || hit_mid.collider != null || hit_bottom.collider != null){
            r2d.isKinematic = true;
            mainCollider.enabled = false;
            isAlive = false;
            Destroy(this.gameObject, 0.3f);
        }

        Debug.DrawRay(top, transform.TransformDirection(Vector2.up) * 0.45f, Color.white);
        Debug.DrawRay(mid, transform.TransformDirection(Vector2.up) * 0.45f, Color.white);
        Debug.DrawRay(bottom, transform.TransformDirection(Vector2.up) * 0.45f, Color.white);
    }

    bool onGround(){
        Bounds colliderBounds;
        float colliderRadius;
        Vector3 groundCheckPos;
        Collider2D[] colliders;

        colliderBounds = mainCollider.bounds;
        colliderRadius = mainCollider.size.x * 0.4f * Mathf.Abs(transform.localScale.x);
        groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);
        
        // Check if player is grounded
        colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius);

        //Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
        bool result = false;
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != mainCollider)
                {
                    result = true;
                    break;
                }
            }
        }

        return result;
    }

    void Flipping(){
        isFacingRight = !isFacingRight;
    }

    bool ValidateComponent(){
        if (mainCollider == null)
            return false;
        return true;
    }

    void DetectPlayer(){
        if (isAttacking)
            return;

        triggerLayer = LayerMask.GetMask("Player");
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.CircleCast(mainCollider.bounds.center, mainCollider.size.x, direction, 5f, triggerLayer);
        
        if (hit.collider != null){
            if (!isWarning && !isAttacking)
                timeWarning = Time.realtimeSinceStartup;

            isWarning = true;
            isRunning = false;
            leftborder.SetActive(false);
            rightborder.SetActive(false);
            maxSpeed = 10.0f;
        }
    }

    void Warning(){
        if (isWarning && Time.realtimeSinceStartup - timeWarning >= 2.0f){
            isAttacking = true;
            isWaitting = false;
            isWarning = false;
        }

        if (isAttacking){
            isWarning = false;
            time = Time.realtimeSinceStartup;
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag == "death_collider"){
            Destroy(this.gameObject);
        }
    }
}
