using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{
    public void OnButtonPress(){
        Time.timeScale = 0;
        SceneManager.LoadScene("Options", LoadSceneMode.Additive);
    }
}
