using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject test;
    public Collider2D range;
    GameObject control;
    bool isSpawn;

    void Awake()
    {
        this.isSpawn = true;
        Debug.Log(range.bounds.max);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            
        //Debug.Log(Time.realtimeSinceStartup);
    }

    void Spawn()
    {
        if (this.isSpawn && control == null) {
            control = Instantiate(test, transform.position, transform.rotation);
            control.transform.GetChild(0).gameObject.SendMessage("SetRangeMin", new Vector2(range.bounds.min.x, range.bounds.min.y));
            control.transform.GetChild(0).gameObject.SendMessage("SetRangeMax", new Vector2(range.bounds.max.x, range.bounds.max.y));
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            Spawn();
            this.isSpawn = false;
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name == "Player" && !isIn(col.gameObject.transform.position))
        {
            this.isSpawn = true;
        }
    }

    bool isIn(Vector2 position)
    {
        if (position.x > range.bounds.min.x && position.x < range.bounds.max.x && position.y > range.bounds.min.y && position.y < range.bounds.max.y)
            return true;
        return false;
    }

    //IEnumerator Testing()
    //{
    //    Debug.Log("Start count");

    //    yield return new WaitForSeconds(5);

    //    Debug.Log("Finished");
    //    this.gameObject.SetActive(false);
    //}

    //void OnCollisionEnter2D(Collision2D col)
    //{
    //    StartCoroutine(Testing());

    //}
}
