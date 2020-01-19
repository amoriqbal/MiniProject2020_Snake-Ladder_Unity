using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBar : MonoBehaviour
{
    public Image loadbar;
    public string sceneToLoad;
    private void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }
    IEnumerator LoadSceneAsync()
    {
        yield return null;
        AsyncOperation loadAsyncOperation=SceneManager.LoadSceneAsync(sceneToLoad);
        while (!loadAsyncOperation.isDone)
        {
            loadbar.fillAmount = loadAsyncOperation.progress;

            yield return new WaitForEndOfFrame();
        }
    }
}
