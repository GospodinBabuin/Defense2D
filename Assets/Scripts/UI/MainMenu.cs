using System.IO;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueGame;
    [SerializeField] private GameObject credits;
    [SerializeField] private SceneLoader sceneLoader;

    
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

        sceneLoader.LoadScene("Game");
    }
    public void ContinueGame()
    {
        sceneLoader.LoadScene("Game");
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
