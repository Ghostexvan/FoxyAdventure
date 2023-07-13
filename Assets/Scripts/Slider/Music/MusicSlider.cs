using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour
{
    public void OnDrag(){
        Slider m_Slider;
        GameObject music;
        Music _music;

        m_Slider = GetComponent<Slider>();
        music = GameObject.FindGameObjectWithTag("Music");

        if (music != null)
            _music = music.GetComponent<Music>();
        else {
            m_Slider.value = 0.0f;
            return;
        }

        _music.ChangeVolume(m_Slider.value);
    }
}
