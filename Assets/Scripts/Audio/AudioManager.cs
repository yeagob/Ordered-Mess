using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] audioSources;

    // Start is called before the first frame update
    void Start()
    {
        CharacterController.walkEvent += PlayWalkAudio;
        CharacterController.walkStopEvent += StopWalkAudio;
        Grab.grabEvent += PlayGrabAudio;
        Grab.throwEvent += PlayThrowAudio;
    }

    private void OnDestroy()
    {
        CharacterController.walkEvent -= PlayWalkAudio;
        CharacterController.walkStopEvent -= StopWalkAudio;
        Grab.grabEvent -= PlayGrabAudio;
        Grab.throwEvent -= PlayThrowAudio;
    }

    private void PlayWalkAudio()
    {
        if (!audioSources[0].isPlaying)
            audioSources[0].Play();
    }

    private void PlayGrabAudio()
    {
        if (!audioSources[1].isPlaying)
            audioSources[1].Play();
    }

    private void PlayThrowAudio()
    {
        if (!audioSources[2].isPlaying)
            audioSources[2].Play();
    }

    private void StopWalkAudio()
    {
        if (audioSources[0].isPlaying)
            audioSources[0].Stop();
    }
}
