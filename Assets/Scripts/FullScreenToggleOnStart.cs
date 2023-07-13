using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenToggleOnStart : MonoBehaviour
{
    Toggle m_Toggle;

    void Awake(){
        m_Toggle = GetComponent<Toggle>();

        if (Screen.fullScreen)
            m_Toggle.isOn = true;
        else
            m_Toggle.isOn = false;
    }
}
