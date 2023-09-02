using System;
using UnityEngine;

public class Fader : MonoBehaviour
{
    private Animator _animator;

    public bool isFading { get; private set; }

    private Action _fadedInCallback;
    private Action _fadedOutCallback;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _animator = GetComponent<Animator>();
        _animator.SetBool("faded", false);
    }


    public void FadeIn(Action fadedInCallback)
    {
        if (isFading)
            return;

        isFading = true;
        _fadedInCallback = fadedInCallback;
        _animator.SetBool("faded", true);
    }

    public void FadeOut(Action fadedOutCallback)
    {
        if (isFading)
            return;

        isFading = true;
        _fadedOutCallback = fadedOutCallback;
        _animator.SetBool("faded", false);
    }

    private void Handle_FadeInAnimationOver()
    {
        _fadedInCallback?.Invoke();
        _fadedInCallback = null;
        isFading = false;
    }

    private void Handle_FadeOutInAnimationOver()
    {
        _fadedOutCallback?.Invoke();
        _fadedOutCallback = null;
        isFading = false;
    }
}
