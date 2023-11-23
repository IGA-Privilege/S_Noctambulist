using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicBox : MonoBehaviour
{
    [SerializeField] private AudioClip preSong;
    [SerializeField] private AudioClip song;
    [SerializeField] private MusicBoxButton button01;
    [SerializeField] private MusicBoxButton button02;
    [SerializeField] private MusicBoxButton button03;
    [SerializeField] private MusicBoxButton button04;
    [SerializeField] private MusicBoxButton button05;
    [SerializeField] private MusicBoxButton button06;
    [SerializeField] private MusicBoxButton button07;
    [SerializeField] private MusicBoxButton button08;

    private AudioSource _audioSource;
    private bool _isWaitingButton02 = false;
    private bool _isWaitingButton03 = false;
    private bool _isWaitingButton04 = false;
    private bool _isWaitingButton05 = false;
    private float _hintTimer = 0f;
    private readonly float _hintInterval = 2.6f;
    [Range(1, 5)] private int _lastHintButtonIndex = 1;
    private float _animTimer = 0f;
    private readonly float _animInterval = 0.75f;
    public bool hasPlayerSolvedPuzzle = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(TickCatHintBeforeSolved());
    }

    public void OnButton01Pressed()
    {
        ResetAllWaitings();
        StartCoroutine(WaitForButton02());
    }

    private IEnumerator WaitForButton02()
    {
        ResetAllWaitings();
        _isWaitingButton02 = true;
        yield return new WaitForSeconds(4f);
        _isWaitingButton02 = false;
    }

    public IEnumerator TickCatHintBeforeSolved()
    {
        while (!hasPlayerSolvedPuzzle)
        {
            if (PlayerController.Instance.isCatView)
            {
                _hintTimer += Time.deltaTime;
                if (_hintTimer > _hintInterval)
                {
                    PlayerButtonAnim(_lastHintButtonIndex);
                    if (_lastHintButtonIndex == 5)
                    {
                        _lastHintButtonIndex = 1;
                    }
                    else
                    {
                        _lastHintButtonIndex++;
                    }
                    _hintTimer = 0f;
                }
            }
            yield return new WaitForSeconds(0);
        }
    }

    private void PlayerButtonAnim(int buttonIndex)
    {
        switch (buttonIndex)
        {
            case 1:
                {
                    button01.StartCoroutine(button01.PlayPressedAudioAndAnim());
                    break;
                }
            case 2:
                {
                    button02.StartCoroutine(button02.PlayPressedAudioAndAnim());
                    break;
                }
            case 3:
                {
                    button03.StartCoroutine(button03.PlayPressedAudioAndAnim());
                    break;
                }
            case 4:
                {
                    button04.StartCoroutine(button04.PlayPressedAudioAndAnim());
                    break;
                }
            case 5:
                {
                    button05.StartCoroutine(button05.PlayPressedAudioAndAnim());
                    break;
                }
        }
    }


    public void OnButton02Pressed()
    {
        if (_isWaitingButton02)
        {
            StartCoroutine(WaitForButton03());
        }
        else
        {
            ResetAllWaitings();
        }
    }

    private IEnumerator WaitForButton03()
    {
        ResetAllWaitings();
        _isWaitingButton03 = true;
        yield return new WaitForSeconds(4f);
        _isWaitingButton03 = false;
    }

    public void OnButton03Pressed()
    {
        if (_isWaitingButton03)
        {
            StartCoroutine(WaitForButton04());
        }
        else
        {
            ResetAllWaitings();
        }
    }

    private IEnumerator WaitForButton04()
    {
        ResetAllWaitings();
        _isWaitingButton04 = true;
        yield return new WaitForSeconds(4f);
        _isWaitingButton04 = false;
    }

    public void OnButton04Pressed()
    {
        if (_isWaitingButton04)
        {
            StartCoroutine(WaitForButton05());
        }
        else
        {
            ResetAllWaitings();
        }
    }

    private IEnumerator WaitForButton05()
    {
        ResetAllWaitings();
        _isWaitingButton05 = true;
        yield return new WaitForSeconds(4f);
        _isWaitingButton05 = false;
    }

    public void OnButton05Pressed()
    {
        if (_isWaitingButton05)
        {
            SolvePuzzle();
        }
        ResetAllWaitings();
    }

    private void SolvePuzzle()
    {
        PlayerInventory.Instance.PlayerGetsCatsPaw();
        hasPlayerSolvedPuzzle = true;
        _audioSource.clip = song;
        _audioSource.PlayDelayed(3f);
        StartCoroutine(TickSongPlayAnimAfterSolved());
    }

    private IEnumerator TickSongPlayAnimAfterSolved()
    {
        yield return new WaitForSeconds(3f);
        while (hasPlayerSolvedPuzzle)
        {
            _animTimer += Time.deltaTime;
            if (_animTimer > _animInterval)
            {
                PressRandomButton();
                _animTimer = 0f;
            }
            yield return new WaitForSeconds(0);
        }
    }

    private void PressRandomButton()
    {
        int randomIndex = UnityEngine.Random.Range(1, 6);
        switch (randomIndex)
        {
            case 1:
                {
                    button01.StartCoroutine(button01.PlayPressedAnim());
                    break;
                }
            case 2:
                {
                    button02.StartCoroutine(button02.PlayPressedAnim());
                    break;
                }
            case 3:
                {
                    button03.StartCoroutine(button03.PlayPressedAnim());
                    break;
                }
            case 4:
                {
                    button04.StartCoroutine(button04.PlayPressedAnim());
                    break;
                }
            case 5:
                {
                    button05.StartCoroutine(button05.PlayPressedAnim());
                    break;
                }
        }
    }

    private void ResetAllWaitings()
    {
        _isWaitingButton02 = false;
        _isWaitingButton03 = false;
        _isWaitingButton04 = false;
        _isWaitingButton05 = false;
    }

}
