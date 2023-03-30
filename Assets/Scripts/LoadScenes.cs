using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScenes : MonoBehaviour
{
   private FaderController _faderController;

    private void Start()
    {
        _faderController = GameObject.FindObjectOfType<FaderController>().GetComponent<FaderController>();
        Debug.Log(_faderController);
    }

    public void LoadScene(string sceneName)
    {
        _faderController.LoadScene(sceneName);
    }
}
