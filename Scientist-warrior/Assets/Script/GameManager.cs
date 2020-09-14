using UnityEngine;

namespace Script
{
    public class GameManager : MonoBehaviour
    {
        public bool gameContinue = true;
        public static GameManager Instance;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
