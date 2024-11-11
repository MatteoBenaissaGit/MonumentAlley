using MatteoBenaissaLibrary.SingletonClassBase;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private int _levelAmount = 1;
    
        public void NextLevel()
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if (currentScene + 1 > _levelAmount)
            {
                currentScene = -1;
            }
            SceneManager.LoadScene(currentScene + 1);
        }
    }
}
