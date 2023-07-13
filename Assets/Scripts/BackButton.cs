using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{

    public void OnButtonPress(){
        Scene current_Scene = SceneManager.GetActiveScene();
        if (current_Scene.name == "Game"){
            SceneManager.UnloadSceneAsync("Options");
            Time.timeScale = 1;
        }
        else
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
