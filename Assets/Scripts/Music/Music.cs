using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public bool isPlaying;
    private static AudioSource _audioSource;
    public AudioClip Background;
    public AudioClip EndCredit;

    private void Awake(){
        if (_audioSource){
            Debug.Log("Duplicate");
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(transform.gameObject);
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = Background;
        _audioSource.Play();
        isPlaying = _audioSource.isPlaying;
    }

    public void PlayMusic(){
        //if (_audioSource.isPlaying)
        //    return;
        
        _audioSource.volume = 1.0f;
        isPlaying = true;
    }

    public void StopMusic(){
        //_audioSource.Stop();
        _audioSource.volume = 0.0f;
        isPlaying = false;
    }

    public void ChangeVolume(float volume){
        _audioSource.volume = volume;
        if (volume == 0.0f){
            isPlaying = false;
        }
        else
            isPlaying = true;
    }

    public float GetVolume(){
        return _audioSource.volume;
    }

    public void PlayEndCredit(){
        _audioSource.clip = EndCredit;
        _audioSource.Play();
    }

    public void PlayBackground(){
        _audioSource.clip = Background;
        _audioSource.Play();
    }

    public float GetRemainEndCreditLength(){
        return EndCredit.length - _audioSource.time;
    }
}
