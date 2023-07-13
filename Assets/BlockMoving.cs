using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMoving : MonoBehaviour
{
    private float time;
    public bool isWaitting,
                isMoving;
    public float maxSpeed = 1.0f;
    public Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = true;
        isWaitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        Waitting();
        Moving();
    }

    void Waitting(){
        if (isMoving)
            return;

        if (Time.realtimeSinceStartup - time >= 5.0f){
            isMoving = true;
            isWaitting = false;
            time = Time.realtimeSinceStartup;
            ChangeDirection();
        }
    }

    void Moving(){
        if (isWaitting)
            return;

        float amntToMove1 = maxSpeed * Time.deltaTime;
        this.transform.Translate(direction * amntToMove1);

        if (Time.realtimeSinceStartup - time >= 2.0f){
            isWaitting = true;
            isMoving = false;
            time = Time.realtimeSinceStartup;
        }
    }

    void ChangeDirection(){
        if (direction == Vector2.left)
            direction = Vector2.right;
        else
            direction = Vector2.left;
    }
}
