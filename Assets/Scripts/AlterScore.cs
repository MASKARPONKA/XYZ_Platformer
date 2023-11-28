using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew
{
    public class AlterScore : MonoBehaviour
    {
        [SerializeField] private int _coins;
        private Hero _hero;

        private void Start()
        {
            _hero = FindObjectOfType<Hero>();
        }
        public void Alter()
        {
            _hero.ScoreAlter(_coins);
        }
    }
}