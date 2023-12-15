using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }
    
    [SerializeField] private Image transitionFadeImage;
    public float transitionFadeTime; 

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SceneTransition(string sceneName)
    {
        transitionFadeImage.DOFade(1.0f, transitionFadeTime).OnComplete(() => {
            SceneManager.LoadScene(sceneName);
            Debug.Log("SceneTransition");
            transitionFadeImage.DOFade(0.0f, transitionFadeTime);
        });
    }
}
