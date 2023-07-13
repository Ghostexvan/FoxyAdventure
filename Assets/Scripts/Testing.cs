using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Testing : MonoBehaviour
{
    #region Declares

    public bool isGrounded,
                isJumping,
                isCrouching,
                isRunning,
                isFacingRight,
                isFalling,
                isAlive,
                isWin,
                isHurt,
                respawn,
                isChecked,
                isClimbing;
    bool keepCrouching;

    [SerializeField] private LayerMask groundLayer;

    private BoxCollider2D mainCollider;
    private CircleCollider2D crouchCollider;

    float moveDirection;
    public float maxSpeed = 3.4f;
    public float crouchSpeed = 2.5f;
    public float jumpHeight = 6.5f;
    public float gravityScale = 1.5f;
    public float lives;
    private float standingHeight;
    private float crouchingHeight;
    public Camera mainCamera;
    public GameObject _speaker;
    public GameObject CheckPoint;

    Vector3 cameraPos;
    public Rigidbody2D r2d;
    Transform t;
    private float time;
    public float hurtTime, checkTime;

    public int maxJump = 2;
    private int jumpAllow;
    Vector2 direction;
    //private float PlayerToSpeaker;

    #endregion

    void Awake(){
        lives = 3;
        isHurt = false;
        respawn = true;
        isChecked = false;
        isClimbing = false;
        CheckPoint.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        #region Get Component

        mainCollider = GetComponent<BoxCollider2D>();
        crouchCollider = GetComponent<CircleCollider2D>();
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

        isAlive = true;
        isFacingRight = t.localScale.x > 0;
        direction = Vector2.right;
        isWin = false;

        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }

        //PlayerToSpeaker = t.position.x - _speaker.transform.position.x;

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        Checked();
        if (isHurt)
            return;

        if (!isAlive || isWin || isClimbing){
            isJumping = isCrouching = isRunning = isFalling = false;
            moveDirection = 0;
            return;
        }
            
        Flipping();
        Run();
        Crouch();
        Jump();  
        Fall();   

        MoveCamera();
    }

    void FixedUpdate(){
        if (isHurt && Time.realtimeSinceStartup - hurtTime >= 0.2f){
            isHurt = false;
            respawn = true;
        }
            
        if (isHurt)
            return;

        CheckOnGround();

        if (!isAlive || isWin || isClimbing){
            isJumping = isCrouching = isRunning = isFalling = false;
            moveDirection = 0;
            return;
        }
        
        Falling();
        
        CheckKeepCrouch();
        
        Moving();
    }

    void Flipping(){
        if (moveDirection > 0 && !isFacingRight){
            isFacingRight = true;
            direction = Vector2.right;
        }
        else if (moveDirection < 0 && isFacingRight){
            isFacingRight = false;
            direction = Vector2.left;
        }
    }

    void CheckOnGround(){
        Vector3[] position = new [] { new Vector3(crouchCollider.bounds.center.x, crouchCollider.bounds.center.y, crouchCollider.bounds.center.z), 
                                      new Vector3(crouchCollider.bounds.max.x - 0.1f, crouchCollider.bounds.center.y, crouchCollider.bounds.center.z),
                                      new Vector3(crouchCollider.bounds.min.x + 0.1f, crouchCollider.bounds.center.y, crouchCollider.bounds.center.z)   };
        for (int i = 0; i < 3; i++){
            groundLayer = LayerMask.GetMask("Ground");
            RaycastHit2D hit = Physics2D.CircleCast(position[i], crouchCollider.radius, Vector2.down, 0.45f, groundLayer);

            Debug.DrawRay(position[0], transform.TransformDirection(Vector2.down) * 0.45f, Color.white);
            Debug.DrawRay(position[1], transform.TransformDirection(Vector2.down) * 0.45f, Color.white);
            Debug.DrawRay(position[2], transform.TransformDirection(Vector2.down) * 0.45f, Color.white);

            if (hit.collider != null){
                //Debug.Log("Grounded");
                isGrounded = true;
                jumpAllow = maxJump;
                isJumping = false;
                return;
            }
            else
                isGrounded = false;
        }   
    }

    void Jump(){
        if (jumpAllow > 0 && Input.GetButtonDown("Jump")){
            //Debug.Log(jumpAllow);
            isJumping = true;
            isFalling = false;
            jumpAllow--;
            r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
        }
    }

    void Fall(){
        if (r2d.velocity.y > 0.0f || isGrounded){
            isFalling = false;
            return;
        }

        isFalling = true;
        isJumping = false;

        Vector3 bottom = new Vector3(crouchCollider.bounds.center.x, crouchCollider.bounds.min.y, 0f);
        Vector3 mid = new Vector3(crouchCollider.bounds.center.x, crouchCollider.bounds.max.y, 0f);
        Vector3 top = new Vector3(mainCollider.bounds.center.x, mainCollider.bounds.max.y, 0f);

        groundLayer = LayerMask.GetMask("Enemies");
        RaycastHit2D hit_top = Physics2D.CircleCast(top, mainCollider.size.x, direction, 0.8f, groundLayer);
        RaycastHit2D hit_mid = Physics2D.CircleCast(mid, crouchCollider.radius, direction, 0.8f, groundLayer);
        RaycastHit2D hit_bottom = Physics2D.CircleCast(bottom, crouchCollider.radius, direction, 0.8f, groundLayer);
    }

    void Falling(){

        if (!isFalling || isGrounded){
            r2d.isKinematic = false;
            return;
        } 

        Vector3 bottom = new Vector3(crouchCollider.bounds.center.x, crouchCollider.bounds.min.y, 0f);
        Vector3 mid = new Vector3(crouchCollider.bounds.center.x, crouchCollider.bounds.max.y, 0f);
        Vector3 top = new Vector3(mainCollider.bounds.center.x, mainCollider.bounds.max.y, 0f);

        groundLayer = LayerMask.GetMask("Ground");
        RaycastHit2D hit_top = Physics2D.CircleCast(top, mainCollider.size.x, direction, 0.45f, groundLayer);
        RaycastHit2D hit_mid = Physics2D.CircleCast(mid, crouchCollider.radius, direction, 0.45f, groundLayer);
        RaycastHit2D hit_bottom = Physics2D.CircleCast(bottom, crouchCollider.radius, direction, 0.45f, groundLayer);

        // if (hit_top.collider == null && hit_mid.collider == null && hit_bottom.collider == null){
        //     r2d.isKinematic = false;
        //     return;
        // }

        // if (hit_top.collider != null || hit_mid.collider != null || hit_bottom.collider != null){
        //     r2d.isKinematic = true;
        //     moveDirection = 0;
        //     float amntToMove1 = gravityScale * Time.deltaTime;
        //     this.transform.Translate(Vector2.down * amntToMove1);
        // }

        Debug.DrawRay(top, transform.TransformDirection(direction) * 0.45f, Color.white);
        Debug.DrawRay(mid, transform.TransformDirection(direction) * 0.45f, Color.white);
        Debug.DrawRay(bottom, transform.TransformDirection(direction) * 0.45f, Color.white);
    }

    void Crouch(){
        if (!isGrounded || (!keepCrouching && Input.GetAxis("Vertical") >= 0)){
            isCrouching = false;
            mainCollider.enabled = true;
            return;
        }

        isCrouching = true;
        moveDirection = 0;
        mainCollider.enabled = false;
    }

    void Run(){
        moveDirection = Input.GetAxis("Horizontal");

        if (moveDirection == 0 || !isGrounded){
            isRunning = false;
            return;
        }

        isRunning = true;
    }

    void Moving(){
        r2d.velocity = new Vector2((moveDirection) * maxSpeed, r2d.velocity.y);
    }

    void CheckKeepCrouch(){
        if (Input.GetAxis("Vertical") < 0){
            keepCrouching = true;
            return;
        }

        groundLayer = LayerMask.GetMask("Ground");
        RaycastHit2D hit = Physics2D.CircleCast(crouchCollider.bounds.center, crouchCollider.radius, Vector2.up, 0.9f, groundLayer);
        if (hit.collider != null)
            keepCrouching = true;
        else
            keepCrouching = false;

        Debug.DrawRay(crouchCollider.bounds.center, transform.TransformDirection(Vector2.up) * 0.9f, Color.white);
    }

    bool ValidateComponent(){
        if (mainCollider == null || crouchCollider == null)
            return false;
        return true;
    }

    void MoveCamera(){
        if (mainCamera)
        {
            mainCamera.transform.position = new Vector3(t.position.x, cameraPos.y, cameraPos.z);
        }

        //if(_speaker){
        //    _speaker.transform.position = new Vector3(t.position.x - PlayerToSpeaker, _speaker.transform.position.y, _speaker.transform.position.z);
        //}
    }

    void OnCollisionEnter2D(Collision2D col){
        Debug.Log(lives);

        if (isHurt)
            return;

        if (col.gameObject.tag == "death_collider" || col.gameObject.tag == "Enemies"){
            //Debug.Log("GG");
            if (!isHurt && lives > 1){
                lives -= 1;
                isHurt = true;
                hurtTime = Time.realtimeSinceStartup;
                return;
            }
            
            if (isAlive && lives == 1){
                isAlive = false;
                isHurt = true;
                Destroy(this.gameObject, 0.2f);
            }
                
        }
    }

    void Checked(){
        if (isChecked && Time.realtimeSinceStartup - checkTime >= 1.0f){
            isChecked = false;
            CheckPoint.SetActive(false);
        }
        else if (isChecked){
            CheckPoint.SetActive(true);
        }
    }
}
