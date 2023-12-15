using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ComicUI
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private int score = 0;
        private bool isGameOver = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            ResetGame();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                QuitGame();
            }
        }

        public void AddScore(int points)
        {
            if (!isGameOver)
            {
                score += points;
            }
        }

        public void GameOver()
        {
            isGameOver = true;
            // Code to show game over screen or end game in some other way
        }

        public void ResetGame()
        {
            score = 0;
            isGameOver = false;
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}