using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicBoxButton : MonoBehaviour
{
    [Range(1, 5)]
    [SerializeField] private int buttonIndex;
    [SerializeField] private MusicBox musicBox;

    private AudioSource _audioSource;
    private Vector3 initialLocalScale;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        initialLocalScale = transform.localScale;
    }

    public void OnPlayerPress()
    {
        if (musicBox.hasPlayerSolvedPuzzle)
        {
            return;
        }

        switch (buttonIndex)
        {
            case 1:
                {
                    musicBox.OnButton01Pressed();
                    StartCoroutine(PlayPressedAudioAndAnim());
                    break;
                }
            case 2:
                {
                    musicBox.OnButton02Pressed();
                    StartCoroutine(PlayPressedAudioAndAnim());
                    break;
                }
            case 3:
                {
                    musicBox.OnButton03Pressed();
                    StartCoroutine(PlayPressedAudioAndAnim());
                    break;
                }
            case 4:
                {
                    musicBox.OnButton04Pressed();
                    StartCoroutine(PlayPressedAudioAndAnim());
                    break;
                }
            case 5:
                {
                    musicBox.OnButton05Pressed();
                    StartCoroutine(PlayPressedAudioAndAnim());
                    break;
                }
        }
    }

    public IEnumerator PlayPressedAudioAndAnim()
    {
        PlayPressedAudio();
        yield return StartCoroutine(PlayPressedAnim());
    }

    public IEnumerator PlayPressedAnim()
    {
        transform.localScale = new Vector3(transform.localScale.x, initialLocalScale.y * 0.85f, transform.localScale.z);
        yield return new WaitForSeconds(0.6f);
        transform.localScale = initialLocalScale;
    }

    private void PlayPressedAudio()
    {
        switch (buttonIndex)
        {
            case 1:
                {
                    _audioSource.clip = musicBox.rhythm01;
                    _audioSource.Play();
                    break;
                }
            case 2:
                {
                    _audioSource.clip = musicBox.rhythm02;
                    _audioSource.Play();
                    break;
                }
            case 3:
                {
                    _audioSource.clip = musicBox.rhythm03;
                    _audioSource.Play();
                    break;
                }
            case 4:
                {
                    _audioSource.clip = musicBox.rhythm04;
                    _audioSource.Play();
                    break;
                }
            case 5:
                {
                    _audioSource.clip = musicBox.rhythm05;
                    _audioSource.Play();
                    break;
                }
        }
    }
}
