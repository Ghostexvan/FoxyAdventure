using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public GameObject player;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "destroyable" /*&& player.GetComponent<PlayerController>().isFall*/)
        {
            Debug.Log(col.gameObject.name);
            col.gameObject.SendMessage("AddDamage", 1);
        }
    }
}
