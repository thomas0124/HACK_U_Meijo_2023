using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public string sceneName;
    public GameObject loadingUi;
    public bool allowNextScene = false;
    private AsyncOperation async;
    public void StartLoad()
    {
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        loadingUi.SetActive(true);
        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
        yield return new WaitUntil(() => allowNextScene);
        async.allowSceneActivation = true;
        allowNextScene = false;
    }
}
