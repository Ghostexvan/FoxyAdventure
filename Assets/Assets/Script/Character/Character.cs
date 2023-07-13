using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] public LayerMask groundLayer;

    public int maxHealth,
               health;
    public float maxSpeed,
                 minSpeed,
                 moveSpeed,
                 moveDirection,
                 gravityScale;
    public bool isHurt,
                isFacingRight,
                isOnGround,
                isMoving,
                isFall;
    public Collider2D mainCollider;
    public Vector2 spawnPoint;
    public Rigidbody2D _physics;
    public Animator _ani;

    public void Awake()
    {
        _physics.freezeRotation = true;
        _physics.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _physics.gravityScale = gravityScale;
        this.spawnPoint = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        this.health = this.maxHealth;
        this.moveSpeed = this.maxSpeed;
        this.CheckOnGround();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    } 

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FixedUpdate()
    {
        this.CheckOnGround();
    }

    public void CheckOnGround()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.3f, Vector2.down, 0.8f, groundLayer);
        if (hit.collider != null)
        {
            this.isOnGround = true;
            this.isFall = false;
        }
        else
        {
            this.isOnGround = false;
            if (_physics.velocity.y < 0)
                this.isFall = true;
            else
                this.isFall = false;
        }
    }

    public void SetMaxHealth(int health)
    {
        if (health < 0)
            return;

        this.health = health;
    }

    public void SetMaxSpeed(float maxSpeed)
    {
        if (maxSpeed < 0)
            return;

        this.maxSpeed = maxSpeed;
    }

    public void SetMinSpeed(float minSpeed)
    {
        if (minSpeed < 0)
            return;

        this.maxSpeed = minSpeed;
    }

    public void SetHealth(int health)
    {
        if (health < 0 || health > this.maxHealth)
            return;

        this.health = health;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        if (moveSpeed < this.minSpeed || moveSpeed > this.maxSpeed)
            return;

        this.moveSpeed = moveSpeed;
    }

    public void SetDirection(bool isFacingRight)
    {
        if (isFacingRight == this.isFacingRight)
            return;

        this.isFacingRight = isFacingRight;
    }

    public void SetOnGround(bool isOnGround)
    {
        if (isOnGround == this.isOnGround)
            return;

        this.isOnGround = isOnGround;
    }

    public void SetMoveDirection(float moveDirection)
    {
        this.moveDirection = moveDirection;
    }

    public void SetSpawnpoint(Vector2 position)
    {
        this.spawnPoint = position;
    }

    public void ChangeDirection()
    {
        this.isFacingRight = !this.isFacingRight;
    }

    public void move()
    {
        _physics.velocity = new Vector2((moveDirection) * moveSpeed, _physics.velocity.y);
    }

    public void move(float moveDirection)
    {
        _physics.velocity = new Vector2((moveDirection) * moveSpeed, _physics.velocity.y);
    }

    public void AddDamage(int damage)
    {
        if (damage < 0 || this.isHurt)
            return;

        this.health -= damage;

        if (this.health <= 0)
        {
            DestroySelf();
        }
        else
        {
            StartCoroutine(this.SetHurt());
        }
    }

    public void DestroySelf()
    {
        StartCoroutine(this.SelfDestruct());
    }

    public IEnumerator SelfDestruct()
    {
        this._physics.gravityScale = 0.0f;
        this.mainCollider.enabled = false;
        this._ani.SetTrigger("Destroy");
        this._physics.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        Destroy(this.transform.gameObject);
    }

    public IEnumerator SetHurt()
    {
        this.isHurt = true;
        this.mainCollider.enabled = false;
        this._physics.isKinematic = true;
        yield return new WaitForSeconds(0.1f);
        this.transform.position = spawnPoint;
        this._physics.isKinematic = false;
        this.isHurt = false;
        this.mainCollider.enabled = true;
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "border")
        {
            this.AddDamage(1);
        }
    }
}
