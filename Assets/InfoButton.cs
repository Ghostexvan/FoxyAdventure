using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoButton : MonoBehaviour
{
    public void OnButtonPress(){
        SceneManager.LoadScene("Credit", LoadSceneMode.Single);
    }
}