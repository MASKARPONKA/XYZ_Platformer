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
            Debug.Log("Your score:" + Score);
        }
    }
}
