using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerAnimation : MonoBehaviour
{
    private Animator _animator;
    private GameObject music;
    private Music _music;

    void Start(){
        _animator = GetComponent<Animator>();
        music = GameObject.FindGameObjectWithTag("Music");

        if (music != null)
            _music = music.GetComponent<Music>();

        SetAnimation();
    }

    void FixedUpdate(){
        SetAnimation();
    }
    
    void OnMouseDown(){
        if (!music)
            return;

        if (_music.isPlaying){
            _music.StopMusic();
        }
        else {
            _music.PlayMusic();
        }
    }

    void SetAnimation(){
        if (music && _music.isPlaying)
            _animator.SetTrigger("On");
        else
            _animator.SetTrigger("Off");
    }
}
