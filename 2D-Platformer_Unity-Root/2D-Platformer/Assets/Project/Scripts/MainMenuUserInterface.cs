using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KyleConibear
{
    public class MainMenuUserInterface : MonoBehaviour
    {
        [SerializeField] private Animator characterAnimator = null;
        public void Play()
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            GameManager.LoadScene(nextSceneIndex, false);
            this.characterAnimator.SetInteger("state", SceneManager.GetActiveScene().buildIndex);
        }

        public void Quit()
        {
            GameManager.ExitApplication();
        }
    }
}