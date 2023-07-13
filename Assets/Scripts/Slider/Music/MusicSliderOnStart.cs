using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSliderOnStart : MonoBehaviour
{
    Slider m_Slider;
    GameObject music;
    Music _music;

    void Awake(){
        m_Slider = GetComponent<Slider>();
        music = GameObject.FindGameObjectWithTag("Music");

        if (music != null)
            _music = music.GetComponent<Music>();
        else {
            m_Slider.value = 0.0f;
            return;
        }

        m_Slider.value = _music.GetVolume();
    }
}
