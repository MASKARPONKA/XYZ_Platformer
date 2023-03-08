using UnityEngine;

namespace PixelCrew
{
    public class ScoreCounter : MonoBehaviour
    {
        private float Score;

        private void Awake()
        {
            Score = 0;
        }
        public void ScoreAlter(float value)
        {
            Score += value;
            if (Score <= 0)
            {
                Score = 0;
            }
            Debug.Log("Your score:" + Score);
        }
    }
}
