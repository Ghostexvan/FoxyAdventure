using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject destinate;
    Vector2 spawnPoint;
    public bool isOn;

    void Awake()
    {
        this.spawnPoint = this.transform.position;
        this.isOn = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (!this.isOn)
        {
            if (this.transform.position.y > this.spawnPoint.y)
            {
                this.transform.Translate(Vector2.down * Time.deltaTime);
            }
        }

    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag != "Player")
            return;

        this.isOn = true;
        if (this.transform.position.y < this.destinate.transform.position.y)
            this.transform.Translate(Vector2.up * Time.deltaTime);
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag != "Player")
            return;

        this.isOn = false;

    }
}
