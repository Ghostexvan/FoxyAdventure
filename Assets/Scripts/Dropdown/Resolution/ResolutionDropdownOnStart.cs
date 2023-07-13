using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionDropdownOnStart : MonoBehaviour
{
    Dropdown m_Dropdown;

    void Awake(){
        m_Dropdown = GetComponent<Dropdown>();
        
        //Debug.Log(Screen.currentResolution.width);
        // switch (Screen.currentResolution.width)
        // {
        //     case 1600:
        //         m_Dropdown.value = 0;
        //         break;
        //     case 1366:
        //         m_Dropdown.value = 1;
        //         break;
        //     case 1280:
        //         m_Dropdown.value = 2;
        //         break;
        //     case 1024:
        //         m_Dropdown.value = 3;
        //         break;
        // }

        if (Screen.width == 1600)
            m_Dropdown.value = 0;
        else if (Screen.width == 1366)
            m_Dropdown.value = 1;
        else if (Screen.width == 1280)
            m_Dropdown.value = 2;
        else if (Screen.width == 1024)
            m_Dropdown.value = 3;
    }
}
