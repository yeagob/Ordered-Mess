using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MyMonoBehaviour
{
    public AudioSource[] audioSources;

    // Start is called before the first frame update
    void Start()
    {
        CharacterController.walkEvent += PlayWalkAudio;
        CharacterController.walkStopEvent += StopWalkAudio;
        Grab.OnGrabEvent += PlayGrabAudio;
        Grab.OnThrowEvent += PlayThrowAudio;
        InGameController.instance.OnStartGame += PlaySortingOutMusic;
        crono.OnCronoCompleted += PlayMessingUpMusic;
        InGameController.instance.OnStartGame += PlayStartEndAudio;
        RoundController.OnGameCompleted += PlayStartEndAudio;
        RoundController.On10SeconsLeft += PlayCountdownAudio;

        //InGameController OnStartGame evento no funciona!!
        PlaySortingOutMusic();
        PlayStartEndAudio();
    }


    private void OnDestroy()
    {
        CharacterController.walkEvent -= PlayWalkAudio;
        CharacterController.walkStopEvent -= StopWalkAudio;
        Grab.OnGrabEvent -= PlayGrabAudio;
        Grab.OnThrowEvent -= PlayThrowAudio;
        InGameController.instance.OnStartGame -= PlaySortingOutMusic;
        crono.OnCronoCompleted -= PlayMessingUpMusic;
        InGameController.instance.OnStartGame -= PlayStartEndAudio;
        RoundController.OnGameCompleted -= PlayStartEndAudio;
    }

    private void PlayWalkAudio()
    {
        if (!audioSources[0].isPlaying)
            audioSources[0].Play();
    }

    private void PlayGrabAudio(HouseProps obj)
    {
        if (!audioSources[1].isPlaying)
            audioSources[1].Play();
    }

    private void PlayThrowAudio(HouseProps obj)
    {
        if (!audioSources[2].isPlaying)
            audioSources[2].Play();
    }

    private void StopWalkAudio()
    {
        if (audioSources[0].isPlaying)
            audioSources[0].Stop();
    }

    private void PlaySortingOutMusic()
    {
        if (!audioSources[4].isPlaying)
        {
            audioSources[4].Play();
            audioSources[4].DOFade(0.8f, 1);
        }

            audioSources[3].DOFade(0, 1f);
    }

    private void PlayMessingUpMusic()
    {
        if (!audioSources[5].isPlaying)
        {
            audioSources[5].Play();
            audioSources[5].DOFade(0.8f, 1);
        }

        audioSources[4].DOFade(0, 1f);
    }

    private void PlayStartEndAudio()
    {
        if (!audioSources[6].isPlaying)
            audioSources[6].Play();
    }

    private void PlayCountdownAudio()
    {
        if (!audioSources[7].isPlaying && !uiController.gameOverPanel.gameObject.activeSelf)
            audioSources[7].Play();
    }
}
