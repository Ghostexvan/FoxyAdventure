using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCheck : MonoBehaviour
{
    public GameObject player, heart1, heart2, heart3, checkpoint;
    public Vector3 position;
    bool endGame;

    void Awake(){
        if (player){
            position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        endGame = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (endGame)
            return;

        if (!player){
            SceneManager.LoadScene("End", LoadSceneMode.Single);
            return;
        }

        if (player.GetComponent<Testing>().respawn){
            player.transform.position = new Vector3(position.x, position.y, position.z);
            player.GetComponent<Testing>().respawn = false;
            player.GetComponent<Testing>().isHurt = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.name.Equals("Player")){
            if (!endGame){
                player.GetComponent<Testing>().isWin = true;
                endGame = true;
                Destroy(GameObject.Find("Speaker"));
                Destroy(heart1);
                Destroy(heart2);
                Destroy(heart3);
                Destroy(checkpoint);
                SceneManager.LoadScene("Credit", LoadSceneMode.Additive);
            }
        }
    }

    public void ChangePosition(Vector3 new_position){
        position = new Vector3(new_position.x, new_position.y, new_position.z);
    }
}
