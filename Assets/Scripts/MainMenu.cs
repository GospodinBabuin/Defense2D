using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueGame;
    public GameObject credits;

    
    private void Start()
    
    {
        if (!File.Exists(Application.persistentDataPath + "/GameSave.save"))
            continueGame.SetActive(false);
        credits.SetActive(false);
    }

    public void StartNewGame()
    {
        if (File.Exists(Application.persistentDataPath + "/GameSave.save"))
            File.Delete(Application.persistentDataPath + "/GameSave.save");
    }
    
    public void Credits()
    {
        if (credits.activeInHierarchy == false)
            credits.SetActive(true);
        else
            credits.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
