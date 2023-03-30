using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private int _reward;
    private bool _isReadyToOpen = true;
    [SerializeField] private AudioSource _openChestSound;
    [SerializeField] private AudioSource _closeChestSound;

    private void Start()
    {
        _animator.SetTrigger("Close");
        _openChestSound = gameObject.AddComponent<AudioSource>() as AudioSource;
        _closeChestSound = gameObject.AddComponent<AudioSource>() as AudioSource;
        _openChestSound.clip = Resources.Load<AudioClip>("Audio/Effects/ChestOpen");
        _closeChestSound.clip = Resources.Load<AudioClip>("Audio/Effects/ChestClose");
    }

    private IEnumerator OpenChest()
    {
        _openChestSound.Play();
        _animator.SetTrigger("Open");
        _isReadyToOpen = false;

        yield return new WaitForSeconds(1);
        
        _closeChestSound.Play();
        _animator.SetTrigger("Close");

        yield return new WaitForSeconds(0.5f);
        _isReadyToOpen = true;

        yield break;
    }

    public void ChestOpen()
    {
        Debug.Log("Open");
        if (_isReadyToOpen)
            StartCoroutine(OpenChest());
    }
}
