using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class MovementController : MonoBehaviour
{
    // Declares
    #region Declares

    public bool isGrounded,
                isJumping,
                isCrouching,
                isRunning,
                isFalling,
                isFacingRight,
                isDead;
    bool keepCrouching;

    [SerializeField] private LayerMask groundLayer;

    private BoxCollider2D mainCollider;
    private CircleCollider2D crouchCollider;

    float moveDirection;
    public float maxSpeed = 3.4f;
    public float crouchSpeed = 2.5f;
    public float jumpHeight = 6.5f;
    public float gravityScale = 1.5f;
    private float standingHeight;
    private float crouchingHeight;
    public Camera mainCamera;

    Vector3 cameraPos;
    Rigidbody2D r2d;
    Transform t;
    private float time;

    #endregion

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

        isJumping = isCrouching = isRunning = keepCrouching = isDead = false;
        if(isGrounded)
            isFalling = false;
        else
            isFalling = true;
        
        isFacingRight = t.localScale.x > 0;

        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        // Get move direction
        if (isDead){
            moveDirection = 0;
            isCrouching = isJumping = isRunning = isFalling = false;
            goto SkipToEnd;
        }
        else {
            moveDirection = Input.GetAxis("Horizontal");

            // Get flag
            isJumping = r2d.velocity.y > 0 ? true : false;
            if (keepCrouching)
                isCrouching = true;
            else
                isCrouching = Input.GetAxis("Vertical") < 0 ? true : false;
            isRunning = moveDirection != 0 ? true : false;
            isFalling = r2d.velocity.y < 0 ? true : false;
        }

        // Flipping
        Flipping();

        // Enabled/Disabled standing collider
        if (!isCrouching)
            mainCollider.enabled = true;

        // Movement
        // First, we check on ground movement
        if (isGrounded){
            // Jumping
            if (Input.GetButtonDown("Jump")){
                r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
                mainCollider.enabled = true;
                goto SkipToEnd;
            }

            // Crouching
            if (isCrouching){
                mainCollider.enabled = false;
                moveDirection = 0;
                // Do something
                goto SkipToEnd;
            }

            // Running
            if (isRunning){
                // Do something
                goto SkipToEnd;
            }

            // Default: Idling
            // Do something
        }
        // Then, we check on air
        else {
            // Jumping
            if (isJumping){
                // Do something
                goto SkipToEnd;
            }

            // Default: Falling
            // Do something
            r2d.velocity = new Vector2(r2d.velocity.x, -gravityScale);
        }
        
        SkipToEnd:
        if (mainCamera)
        {
            mainCamera.transform.position = new Vector3(t.position.x, cameraPos.y, cameraPos.z);
        }
    }

    void FixedUpdate(){
        isGrounded = onGround();

        if (isDead){
            Dead();
        }

        if (isCrouching){
            groundLayer = LayerMask.GetMask("Ground");
            RaycastHit2D hit = Physics2D.CircleCast(crouchCollider.bounds.center, crouchCollider.radius, Vector2.up, 0.8f, groundLayer);
            if (hit.collider != null)
                keepCrouching = true;
            else
                keepCrouching = false;

            //Debug.DrawRay(crouchCollider.bounds.center, transform.TransformDirection(Vector2.up) * hit.distance, Color.white);
        }

        r2d.velocity = new Vector2((moveDirection) * maxSpeed, r2d.velocity.y);
    }

    bool ValidateComponent(){
        if (mainCollider == null || crouchCollider == null)
            return false;
        return true;
    }

    bool onGround(){
        //groundLayer = LayerMask.GetMask("Ground");
        RaycastHit2D hit = Physics2D.CircleCast(crouchCollider.bounds.center, crouchCollider.radius, Vector2.down, 0.5f, groundLayer);

        if (hit.collider != null)
            return true;
        return false;
    }

    void Flipping(){
        if (moveDirection > 0 && !isFacingRight){
            isFacingRight = true;
        }
        else if (moveDirection < 0 && isFacingRight){
            isFacingRight = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag == "death_collider" || col.gameObject.tag == "Enemies"){
            Debug.Log("GG");
            isDead = true;
            Destroy(this.gameObject, 0.2f);
        }
    }

    void Dead(){
        if (!isDead){
            time = Time.realtimeSinceStartup;
        }
        else if (Time.realtimeSinceStartup - time >= 1.0f){
            Destroy(this.gameObject, 0.2f);
            StartCoroutine(DelayAction(2));
            SceneManager.LoadScene("End", LoadSceneMode.Single);
        }
    }

    IEnumerator DelayAction(float delayTime)
    {
    //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);
    
    //Do the action after the delay time has finished.
    }
}
