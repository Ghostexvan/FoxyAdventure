using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    void Awake(){
        Screen.SetResolution(1366, 768, FullScreenMode.Windowed, 60);
    }
}
