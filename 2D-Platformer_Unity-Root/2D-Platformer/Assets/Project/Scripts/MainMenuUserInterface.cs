using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KyleConibear
{
    public class MainMenuUserInterface : MonoBehaviour
    {
        [SerializeField] private int loadSceneIndex = 1;
        [SerializeField] private Animator characterAnimator = null;
        [SerializeField] private string characterRunStateName = "PlayerMainMenuRun_Animation";
        public void Play()
        {
            this.characterAnimator.Play(this.characterRunStateName);
            GameManager.LoadScene(this.loadSceneIndex);
        }

        public void Quit()
        {
            GameManager.ExitApplication();
        }
    }
}