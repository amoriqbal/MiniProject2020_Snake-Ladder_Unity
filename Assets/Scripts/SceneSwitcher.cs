using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneSwitcher : MonoBehaviour
{
    private GameObject BWPoints, SceneObjectsHolder;
    public string nextSceneName;
    private Scene nextScene;

    private void Start()
    {
        BWPoints = BoardWaypoints.Instance.gameObject;
        SceneObjectsHolder = GameObject.FindGameObjectWithTag("SceneObjectsHolder");
    }
    public void StartMatchCurrentBoard()
    {
        //SceneManager.sceneLoaded += ;
        StartCoroutine(Scene_load());
    }
    IEnumerator Scene_load()
    {
        SceneObjectsHolder.SetActive(false);
        Scene thisScene = SceneManager.GetActiveScene();
        AsyncOperation ao = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        while (!ao.isDone)
            yield return new WaitForSeconds(0.1f);
        Debug.Log("Match scene Loaded!!");
        nextScene = SceneManager.GetSceneByName(nextSceneName);
        SceneManager.MoveGameObjectToScene(BWPoints, nextScene);
        SceneManager.SetActiveScene(nextScene);
        
        yield return SceneManager.UnloadSceneAsync(thisScene);
    }
}
