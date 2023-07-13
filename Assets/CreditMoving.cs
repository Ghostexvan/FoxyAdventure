using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditMoving : MonoBehaviour
{
    public float maxSpeed; 
    public bool isMoving;
    public float time;
    private GameObject music;
    private Music _music;

    // Start is called before the first frame update
    void Awake()
    {
        maxSpeed = 0.5f;
        isMoving = true;
        music = GameObject.FindGameObjectWithTag("Music");

        Scene current_Scene = SceneManager.GetActiveScene();
        if (current_Scene.name == "Game"){
            Debug.Log("Loaded");
            GameObject background =  GameObject.FindGameObjectWithTag("NormalCredit");
            background.SetActive(false);

            if (music != null){
            _music = music.GetComponent<Music>();
            _music.PlayEndCredit();
            }
        }
        else{
            GameObject background =  GameObject.FindGameObjectWithTag("WinCredit");
            background.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        if (isMoving){
            float amntToMove1 = maxSpeed * Time.deltaTime;
            this.transform.Translate(Vector2.up* amntToMove1);
        }
        
        if ((!isMoving && Time.realtimeSinceStartup >= time) || Input.GetButtonDown("Jump")){
            if (_music)
                _music.PlayBackground();
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }

    void OnTriggerStay2D(Collider2D col){
        if (col.gameObject.name.Equals("EndCredit") && col.gameObject.transform.position.y <= this.gameObject.transform.position.y){
            if (isMoving){
                isMoving = false;
                time = Time.realtimeSinceStartup + _music.GetRemainEndCreditLength();
                return;
            }
        }
    }
}
