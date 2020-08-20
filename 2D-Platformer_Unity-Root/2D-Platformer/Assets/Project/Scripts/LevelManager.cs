using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KyleConibear
{
    using static Logger;

    public class LevelManager : MonoBehaviour
    {
        [Header("General")]
        public bool isLogging = false;

        public static Action<string, LevelManager> On_LevelLoaded;

        [SerializeField] private LevelUserInterface levelUI = null;
        private LevelUserInterface LevelUI
        {
            get
            {
                if (levelUI != null)
                {
                    return this.levelUI;
                }
                else
                {
                    levelUI = GameObject.FindObjectOfType<LevelUserInterface>();

                    if (levelUI == null)
                    {
                        Logger.Log(this.isLogging, Type.Error, $"LevelUI is {levelUI}.\n(Link in inspector.)");
                    }

                    return levelUI;
                }
            }
        }

        [SerializeField] private Animator characterAnimator = null;
        [SerializeField] private Player player = null;

        [SerializeField] private Collider2D playAreaCollider = null;

        private int time = 300;
        private int gemCount = 0;
        private int cherryCount = 0;

        public void SetPlayerActive()
        {
            this.player.gameObject.SetActive(true);
        }

        private void PlayerHurt()
        {
            StartCoroutine(PlayerHurtDelay());
        }

        private IEnumerator PlayerHurtDelay()
        {
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(0);
        }

        public void IncreaseTime(int amount)
        {
            this.time += amount;
        }
        public void IncrementGemCount()
        {
            gemCount++;
            Logger.Log(this.isLogging, Type.Message, $"gemCount: {gemCount}");
            this.LevelUI.UpdateGemCounter(gemCount);
        }
        public void IncrementCherryCount()
        {
            cherryCount++;
            Logger.Log(this.isLogging, Type.Message, $"cherryCount: {cherryCount}");
            this.LevelUI.UpdateCherryCounter(cherryCount);
        }

        private IEnumerator DecrementTime(float delay = 0)
        {
            yield return new WaitForSeconds(1);
            this.time--;
            LevelUI.UpdateTimeCounter(this.time);
            if (player.state != Player.State.Hurt)
            {
                StartCoroutine(DecrementTime());
            }
        }

        public bool IsTargetWithinPlayArea(Vector3 target)
        {
            return this.playAreaCollider.bounds.Contains(target);
        }

        private void Start()
        {
            if (On_LevelLoaded != null)
            {
                On_LevelLoaded.Invoke(SceneManager.GetActiveScene().name, this);
            }
            else
            {
                Logger.Log(this.isLogging, Type.Warning, $"On_LevelLoaded Action is null.");
            }

            Player.OnPlayerHurt += this.PlayerHurt;

            this.characterAnimator.SetInteger("state", SceneManager.GetActiveScene().buildIndex);

            //this.characterAnimator.Play("Player-LevelRunIn_Animation");
            StartCoroutine(DecrementTime(3));
        }

        private void OnDisable()
        {
            Player.OnPlayerHurt -= this.PlayerHurt;
        }
    }
}