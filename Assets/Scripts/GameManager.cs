using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
        private Grumpy _grumpy;
        private int _score;
        

        private void Start()
        {
                _score = 0;
                _grumpy = FindObjectOfType<Grumpy>();
                _grumpy.OnUpdateScore += UpdateScore;
                _grumpy.OnHitPipe += HitPipe;
        }

        private void HitPipe()
        {
                Debug.Log("Hit Pipe");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void UpdateScore()
        { 
                _score++;
               Debug.Log($"Score {_score}");
        }
}
