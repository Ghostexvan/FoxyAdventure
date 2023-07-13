using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : DogController1
{
    public GameObject bullet;
    public Collider2D trigger;
    public bool isReturn, isCooldown, isAttack;
    public Vector2 targetPosition;

    new void Awake()
    {
        this._physics.isKinematic = true;
        base.Awake();
    }

    new public void FixedUpdate()
    {
        base.FixedUpdate();
        this.CheckWall();
        this.CheckGround();
        this.CheckRangeHorizontal();
        HandleMove();
        this.Flipping();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag != "Player")
            return;

        this.isAttack = true;

        if (!this.isReturn && !this.isCooldown)
        {
            this.targetPosition = col.gameObject.transform.position;
        }    
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag != "Player")
            return;

        this.targetPosition = this.spawnPoint;
        this.isReturn = true;
    }

    new public void HandleMove()
    {
        if (!this.isReturn && (!this.isAttack || this.isCooldown))
            return;

        this.GetMoveDirection();

        if (this.moveDirection == 0)
        {
            if (this.isReturn)
            {
                this.isAttack = false;
                this.isReturn = false;
            }
            else
            {
                StartCoroutine(this.Cooldown());
            }

            Instantiate(bullet, new Vector3(this.transform.position.x, this.transform.position.y - 1.2f, this.transform.position.z), Quaternion.identity);
            this.isMoving = false;
        }
            
        else if (!this.isCooldown)
        {
            this.isMoving = true;
        }

        this.move();
    }

    public IEnumerator Cooldown()
    {
        this.isCooldown = true;
        yield return new WaitForSeconds(2f);
        this.isCooldown = false;
        //this.GetMoveDirection();
    }

    new public void CheckWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(this.moveDirection, 0), 1.0f, this.groundLayer);
        if (hit.collider != null)
        {
            Debug.Log("Wall hit!");
            this.targetPosition = this.spawnPoint;
            this.isReturn = true;
            this.isCooldown = false;
        }
    }

    new public void CheckGround()
    {
        
    }

    new public void CheckRangeHorizontal()
    {
        if ((this.transform.position.x <= this.min.x) || (this.transform.position.x >= this.max.x))
        {
            Debug.Log("Out of range!");
            this.targetPosition = this.spawnPoint;
            this.isReturn = true;
            this.isCooldown = false;
        }
    }

    void GetMoveDirection()
    {
        if (this.isCooldown)
            return;

        this.isFacingRight = this.transform.position.x < this.targetPosition.x ? false : true;

        if (Mathf.Abs(this.transform.position.x - this.targetPosition.x) >= 0.1f)
            this.moveDirection = this.isFacingRight ? -1 : 1;
        else
            this.moveDirection = 0;
    }
}
