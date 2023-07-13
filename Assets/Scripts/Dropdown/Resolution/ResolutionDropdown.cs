using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionDropdown : MonoBehaviour
{
    public void OnChangeValue(){
        Dropdown m_Dropdown;
        m_Dropdown = GetComponent<Dropdown>();
        if (!m_Dropdown)
            return;

        switch (m_Dropdown.value)
        {
            case 0:
                Screen.SetResolution(1600, 900, Screen.fullScreen);
                break;
            case 1:
                Screen.SetResolution(1366, 768, Screen.fullScreen);
                break;
            case 2:
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                break;
            case 3:
                Screen.SetResolution(1024, 576, Screen.fullScreen);
                break;
        }
    }
}
