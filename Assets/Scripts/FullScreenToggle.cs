using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenToggle : MonoBehaviour
{
    public void setFullScreen(){
        Toggle m_Toggle = GetComponent<Toggle>() ;

        if (!Screen.fullScreen && m_Toggle.isOn)
            //Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.ExclusiveFullScreen, 60);
        else if (Screen.fullScreen && !m_Toggle.isOn)
        {
            //Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.Windowed, 60);
        }
    }
}
