using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

namespace ComicUI
{
    public class LoadSceneAfterDelay : MonoBehaviour
    {
        public string sceneName;
        public Image loadingImage;

        void Start()
        {
            StartCoroutine(LoadSceneDelay());
        }

        private IEnumerator LoadSceneDelay()
        {
            float duration = 2f;
            float count = 0;

            while (duration > count)
            {
                count += Time.deltaTime;
                loadingImage.fillAmount = count;
                yield return null;
            }

            SceneManager.LoadScene(sceneName);
        }
    }
}