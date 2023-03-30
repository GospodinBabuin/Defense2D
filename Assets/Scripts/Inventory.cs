using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    private int _money;
    private AudioSource _audioSource;
    public int Money 
    {
        get
        {
            return _money;
        }
        set
        {
            _money = value;
            _moneyCountText.text = _money.ToString();
        }
    }
    public enum BannerState { ON, OFF }
    public BannerState _bannerState;

    private GameObject _resourcesField; 
    private Text _moneyCountText;

    private Animator _animator;
    public bool isClosed = true;


    private void Start()
    {
        if (gameObject.CompareTag("PlayerResources"))
        {
            Instance = this;

            _resourcesField = GameObject.Find("Resources");
            _moneyCountText = GetComponentInChildren<Text>();
            _resourcesField.SetActive(false);
            _bannerState = BannerState.OFF;
            _audioSource = GetComponentInChildren<AudioSource>();
        }

        _animator = GetComponentInChildren<Animator>();
        _animator.SetTrigger("Close");
    }

    public void ShowOrHideResources()
    {
        if (_bannerState == BannerState.OFF)
        {
            _resourcesField.SetActive(true);
            _animator.SetTrigger("Open");
            isClosed = false;
            _bannerState = BannerState.ON;
        }
        else
        if (_bannerState == BannerState.ON)
        {
            _resourcesField.SetActive(false);
            _animator.SetTrigger("Close");
            isClosed = true;
            _bannerState = BannerState.OFF;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("PlayerResources"))
        {
            if (collision.CompareTag("Drop"))
            {
                if (isClosed == true)
                    _animator.SetTrigger("Close");
                if (_audioSource.isPlaying == false)
                {
                    _audioSource.pitch = Random.Range(1f, 1.5f);
                    _audioSource.volume = Random.Range(0.8f, 1f);
                    _audioSource.Play();
                }
                
                Money++;
                Destroy(collision.gameObject);
            }
        }
    }
}
