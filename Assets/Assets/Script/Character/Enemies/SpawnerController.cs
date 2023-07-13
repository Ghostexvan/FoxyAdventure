using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject mob, player, minPoint, maxPoint;
    public int player_health;
    public Collider2D range;
    GameObject control;
    public bool isSpawn,
                isVisible,
                isPlayerInRange;

    void Awake()
    {
        this.isSpawn = true;
        
    }

    void Start()
    {
        player_health = player.GetComponent<PlayerController1>().health;
    }

    void FixedUpdate()
    {
        this.CheckPlayer();
    }

    void Spawn()
    {
        if (this.isSpawn && control == null)
        {
            control = Instantiate(mob, transform.position, transform.rotation);
            control.SendMessage("SetRangeMin", new Vector2(minPoint.transform.position.x, minPoint.transform.position.y));
            control.SendMessage("SetRangeMax", new Vector2(maxPoint.transform.position.x, maxPoint.transform.position.y));
            control.SendMessage("SetSpawner", this.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Spawn();
            this.isSpawn = false;
            this.isPlayerInRange = true;
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && !isIn(col.gameObject.transform.position))
        {
            this.isSpawn = true;
            this.isPlayerInRange = false;
        }
    }

    bool isIn(Vector2 position)
    {
        if (position.x > range.bounds.min.x && position.x < range.bounds.max.x && position.y > range.bounds.min.y && position.y < range.bounds.max.y)
            return true;
        return false;
    }

    void CheckPlayer()
    {
        if (player == null || control == null)
            return;

        if (player_health != player.GetComponent<PlayerController1>().health)
        {
            Debug.Log("Player get damaged!");
            Vector2 player_position = player.transform.position;
            if (player_position == player.GetComponent<PlayerController1>().spawnPoint)
            {
                player_health = player.GetComponent<PlayerController1>().health;
                this.DestroyMob();
            }
            
        }

        if (!this.isVisible && !this.isPlayerInRange)
        {
            Debug.Log("Mob not visible");
            this.DestroyMob();
        }
    }

    public void SetVisible(bool isVisible)
    {
        this.isVisible = isVisible;
    }

    void DestroyMob()
    {
        if (control != null)
        {
            control.SendMessage("DestroySelf");
        }
    }
}
