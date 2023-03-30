using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;

    public AudioSource backgroundMusic;
    public AudioSource musicTransition;
    public List<AudioClip> daytimeMusic;
    public List<AudioClip> nighttimeMusic;
    public int dayMusicIndex = 0;
    public int nightMusicIndex = 0;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ChangeClip(ref dayMusicIndex, daytimeMusic);
        backgroundMusic.Play();
    }
    
    public void PlayDaytimeMusic()
    {
        musicTransition.Play();
        //StartCoroutine(VolumeDown(backgroundMusic));
        ChangeClip(ref dayMusicIndex, daytimeMusic);
        backgroundMusic.Play();
    }
    public void PlayNighttimeMusic()
    {
        musicTransition.Play();
        //StartCoroutine(VolumeDown(backgroundMusic));
        ChangeClip(ref nightMusicIndex, nighttimeMusic);
        backgroundMusic.Play();
    }
    /*
    private IEnumerator VolumeUp(AudioSource music)
    {
        while (music.volume < 1)
        {
            music.volume += 0.1f;
            yield return new WaitForSeconds(1);
        }
    }
    private IEnumerator VolumeDown(AudioSource music)
    {
        while (music.volume > 0)
        {
            music.volume -= 0.1f;
            yield return new WaitForSeconds(1);
        }        
        StartCoroutine(VolumeUp(backgroundMusic));
    }
    */
    
    private void ChangeClip(ref int index, IReadOnlyList<AudioClip> music)
    {
        if (index < music.Count)
        {
            backgroundMusic.clip =  music[index];
            index++;
        }
        else
        {
            index = 0;
            backgroundMusic.clip =  music[index];
        }
    }
}
