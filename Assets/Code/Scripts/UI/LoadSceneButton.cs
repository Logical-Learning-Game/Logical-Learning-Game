using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI
{
    public class LoadSceneButton : MonoBehaviour
    {
        public string SceneName;

        public void LoadScene()
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}
