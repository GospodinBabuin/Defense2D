using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    private FaderController _faderController;
    public SceneLoader Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _faderController = GetComponentInChildren<FaderController>();
    }

    public void LoadScene(string sceneName)
    {
        _faderController.LoadScene(sceneName);
    }
}
