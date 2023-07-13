using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapsMask : MonoBehaviour
{
    public Collider2D range;
    public Renderer map;

    void OnTriggerStay2D(Collider2D col)
    {
        //Debug.Log(col.gameObject.transform.position.x + " : " + (this.gameObject.transform.position.x - this.gameObject.GetComponent<Renderer>().bounds.size.x/2));

        if (col.gameObject.tag == "Player")
        {
            map.enabled = false;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            bool isIn = this.isIn(col.gameObject.transform.position);
            Debug.Log(isIn);
            if (isIn)
                return;

            map.enabled = true;
        }
    }

    bool isIn(Vector2 position)
    {
        if (position.x > range.bounds.min.x && position.x < range.bounds.max.x && position.y > range.bounds.min.y && position.y < range.bounds.max.y)
            return true;
        return false;
    }
}
