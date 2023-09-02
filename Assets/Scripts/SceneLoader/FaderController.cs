using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FaderController : MonoBehaviour
{
    private bool _isLoading;
    private Fader _fader;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _fader = GetComponentInChildren<Fader>();
    }

    public void LoadScene(string sceneName)
    {
        if (_isLoading)
            return;

        var currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == sceneName)
            throw new System.Exception("scene already loaded");

        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        _isLoading = true;

        bool waitFading = true;
        _fader.FadeIn(() => waitFading = false);

        while (waitFading)
            yield return null;

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
            yield return null;

        async.allowSceneActivation = true;

        waitFading = true;
        _fader.FadeOut(() => waitFading = false);

        while (waitFading)
            yield return null;

        _isLoading = false;
    }
}
