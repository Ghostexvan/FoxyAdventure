using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController1 : Character
{
    [SerializeField] public LayerMask enemiesLayer;
    public float jumpHeight,
                 cameraDistance;
    public bool isCrouch,
                isJump,
                isWin;
    public CircleCollider2D crouchCollider;
    public Camera mainCamera;
    Vector3 cameraPos;
    public PlayerInput playerInput;
    public GameObject checkPointText,
                      heart1, heart2, heart3;

    new void Awake()
    {
        base.Awake();
        this.isFacingRight = true;
        this.isCrouch = false;
        this.isJump = false;
        this.SetAnimation();
        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }
        this.checkPointText.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        this.SetAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        this.SetAnimation();
        this.Flipping();
        if (isWin)
        {
            this.moveDirection = 0;
            return;
        }

        this.MoveCamera();
        this.ControllingInput();
        
        this.SetHeart();
    }

    new void FixedUpdate()
    {
        this.ControllingInput();
        if (isWin)
        {
            this.moveDirection = 0;
            this.isMoving = this.isJump = this.isFall = false;
            this.isFacingRight = false;
            return;
        }

        base.FixedUpdate();
        this.ControllingAction();
        this.move();
    }

    void MoveCamera()
    {
        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
            float distance = cameraPos.y - this.gameObject.transform.position.y;
            if (distance <= 0.0f)
            {
                mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, cameraPos.z);
            }
            else if (distance >= cameraDistance)
            {
                mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y + cameraDistance, cameraPos.z);
                return;
            }
            else
                mainCamera.transform.position = new Vector3(transform.position.x, cameraPos.y, cameraPos.z);
        }
    }

    void SetAnimation()
    {
        if (this.isHurt)
        {
            _ani.SetTrigger("Hurt");
            return;
        }
            
        if (this.isCrouch)
        {
            _ani.SetTrigger("Crouch");
            return;
        }

        if (this.isJump)
        {
            _ani.SetTrigger("Jump");
            return;
        }

        if (this.isFall)
        {
            _ani.SetTrigger("Fall");
            return;
        }

        if (this.isMoving)
        {
            _ani.SetTrigger("Run");
            return;
        }

        _ani.SetTrigger("Idle");
    }

    void Flipping()
    {
        if (this.isFacingRight)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void ControllingInput()
    {
        var walk = playerInput.actions["Walk"];
        if (walk.WasReleasedThisFrame())
            this.moveSpeed = this.maxSpeed;

        var crouch = playerInput.actions["Crouch"];
        if (!crouch.IsPressed() && CheckKeepCrouch())
        {
            this.mainCollider.enabled = true;
            this.moveSpeed = this.maxSpeed;
            this.isCrouch = false;
        }

        var move = playerInput.actions["Move"];
        if (move.WasReleasedThisFrame())
            this.isMoving = false;
    }

    void ControllingAction()
    {
        if (this.isOnGround)
        {
            this.isFall = false;

            if (this._physics.velocity.y <= 0)
            {
                this.isJump = false;
            }
        }

        if (this.isOnGround && this.isCrouch)
        {
            this.moveSpeed = this.minSpeed;
        }

        if (this.isJump && this.isFall)
        {
            this.isJump = false;
        }

        if (!this.isOnGround)
        {
            if (_physics.velocity.y > 0)
            {
                this.isJump = true;
                this.isFall = false;
            }
            else if (_physics.velocity.y < 0)
            {
                this.isJump = false;
                this.isFall = true;
            }
            else
            {
                this.isJump = false;
                this.isFall = false;
            }
        }
    }

    bool CheckKeepCrouch()
    {
        RaycastHit2D hit = Physics2D.CircleCast(crouchCollider.bounds.center, crouchCollider.radius, Vector2.up, 1.0f, this.groundLayer);
        Debug.DrawLine(crouchCollider.bounds.center, hit.point, Color.red);
        if (hit.collider != null)
            return false;
        else
            return true;
    }

    void HandleMove()
    {
        if (this.isHurt)
            return;

        this.isMoving = true;

        if (this.moveDirection > 0)
            this.isFacingRight = true;
        else if (this.moveDirection < 0)
            this.isFacingRight = false;
    }

    void HandleJump()
    {
        if (this.isHurt)
            return;

        if (this.isOnGround)
        {
            this.isJump = true;
            this._physics.velocity = new Vector2(this._physics.velocity.x, this.jumpHeight);
        }
    }

    void HandleCrouch()
    {
        if (this.isHurt)
            return;

        isCrouch = true;
        this.mainCollider.enabled = false;
    }

    public void OnMove(InputValue input)
    {
        if (this.isHurt || this.isWin)
        {
            this.moveDirection = 0;
            return;
        }

        Vector2 inputVec = input.Get<Vector2>();
        this.moveDirection = input.Get<Vector2>().x;

        HandleMove();
    }

    public void OnWalk()
    {
        if (this.isWin)
        {
            this.moveDirection = 0;
            return;
        }
            

        this.moveSpeed = this.minSpeed;
    }

    public void OnJump()
    {
        if (this.isWin)
            return;

        HandleJump();
    }

    public void OnCrouch()
    {
        if (this.isWin)
            return;

        HandleCrouch();
    }

    new public void SetSpawnpoint(Vector2 position)
    {
        this.spawnPoint = position;
        StartCoroutine(this.DisplayCheckpointText());
    }

    new IEnumerator SelfDestruct()
    {
        this.crouchCollider.enabled = false;
        yield return base.SelfDestruct();
    }

    new public IEnumerator SetHurt()
    {
        this.crouchCollider.enabled = false;
        yield return base.SetHurt();
        this.crouchCollider.enabled = true;
    }

    public IEnumerator DisplayCheckpointText()
    {
        this.checkPointText.SetActive(true);
        yield return new WaitForSeconds(2f);
        this.checkPointText.SetActive(false);
    }

    void SetHeart()
    {
        if (this.health >= 3)
        {
            this.heart3.SetActive(true);
        }
        else
            this.heart3.SetActive(false);

        if (this.health >= 2)
        {
            this.heart2.SetActive(true);
        }
        else
            this.heart2.SetActive(false);

        if (this.health >= 1)
        {
            this.heart1.SetActive(true);
        }
        else
            this.heart1.SetActive(false);
    }

    public void SetIsWin(bool isWin)
    {
        this.isWin = isWin;
    }
}
