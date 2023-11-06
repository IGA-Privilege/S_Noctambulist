using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheaterBox : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayCatAnimation()
    {
        _animator.SetTrigger("Cat");
    }

    public void PlayGirlAnimation()
    {
        _animator.SetTrigger("Girl");
    }
}
