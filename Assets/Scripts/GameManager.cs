using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
        public GameObject gameOverCanvas;
        public GameObject scoreCanvas;
        private Grumpy _grumpy;
        private int _score;
        private Text _scoreTextReference;

        private void Start()
        {
                scoreCanvas.SetActive(true);
                gameOverCanvas.SetActive(false);
                Time.timeScale = 1;
                _score = 0;
                _scoreTextReference = scoreCanvas.GetComponentInChildren<Text>();
                _grumpy = FindObjectOfType<Grumpy>();
                _grumpy.OnUpdateScore += UpdateScore;
                _grumpy.OnHitPipe += HitPipe;
        }

        private void GameOver()
        {
                gameOverCanvas.SetActive(true);
                Time.timeScale = 0; // pauses game
        }

        public void Replay()
        {
                // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                SceneManager.LoadScene(sceneBuildIndex: 0);
        }
        
        private void HitPipe()
        {
                GameOver();
        }

        private void UpdateScore()
        { 
                _score++;
                _scoreTextReference.text = _score.ToString();
                Debug.Log($"Score {_score}");
        }

        public int GetScore() => _score;
}
