using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController1 : Character
{
    public Vector2 min, max;
    public GameObject spawner;

    new void Awake()
    {
        base.Awake();
        this.isFacingRight = false;
        this.isMoving = this.isOnGround ? true : false;
        this.SetAnimation();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.SetAnimation();
    }

    // Update is called once per frame
    public void Update()
    {
        this.SetAnimation();
    }

    new public void FixedUpdate()
    {
        base.FixedUpdate();
        this.CheckWall();
        this.CheckGround();
        this.CheckRangeHorizontal();
        HandleMove();
    }

    public void SetAnimation()
    {
        if (this.isMoving)
        {
            _ani.SetTrigger("Run");
            return;
        }

        _ani.SetTrigger("Idle");
    }

    public void HandleMove()
    {
        if (this.isOnGround)
        {
            this.move();
            this.isMoving = true;
            this.moveDirection = this.isFacingRight ? 1 : -1;
        }
        else
        {
            this.isMoving = false;
        }
    }

    public void CheckWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(this.moveDirection, 0), 1.0f, this.groundLayer);
        if (hit.collider != null)
        {
            this.ChangeDirection();
            this.Flipping();
        }
    }

    public void CheckGround()
    {
        if (!this.isOnGround)
            return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(this.moveDirection, -2), 2.0f, this.groundLayer);

        if (hit.collider == null)
        {
            this.ChangeDirection();
            this.Flipping();
        }
    }

    public void CheckRangeHorizontal()
    {
        if ((this.transform.position.x <= this.min.x && !this.isFacingRight) || (this.transform.position.x >= this.max.x && this.isFacingRight))
        {
            Debug.Log("Out of range!");
            this.ChangeDirection();
            this.Flipping();
        }
    }

    public void Flipping()
    {
        if (!this.isFacingRight)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    public void SetRangeMin(Vector2 min)
    {
        this.min = min;
    }

    public void SetRangeMax(Vector2 max)
    {
        this.max = max;
    }

    public void SetSpawner(GameObject spawner)
    {
        this.spawner = spawner;
    }

    public void OnBecameInvisible()
    {
        spawner.SendMessage("SetVisible", false);
    }

    public void OnBecameVisible()
    {
        spawner.SendMessage("SetVisible", true);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Hit player!");
            col.gameObject.SendMessage("AddDamage", 1);
        }
    }
}
