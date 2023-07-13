using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public GameObject player, heart1, heart2, heart3, checkpoint;
    bool endGame;

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

        if (!player)
        {
            SceneManager.LoadScene("End", LoadSceneMode.Single);
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (!endGame)
            {
                //player.GetComponent<Testing>().isWin = true;
                endGame = true;
                Destroy(GameObject.Find("Speaker"));
                Destroy(heart1);
                Destroy(heart2);
                Destroy(heart3);
                Destroy(checkpoint);
                player.SendMessage("SetIsWin", true);
                SceneManager.LoadScene("Credit", LoadSceneMode.Additive);
            }
        }
    }
}
