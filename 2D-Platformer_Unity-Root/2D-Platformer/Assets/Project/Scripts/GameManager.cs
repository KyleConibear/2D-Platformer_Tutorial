using UnityEngine;
using UnityEngine.SceneManagement;

namespace KyleConibear
{
    using static Logger;
    /// <summary>
    /// The purpose of the GameManager is to be a persistent class that will exist throughout the life of the application.
    /// </summary>
    public static class GameManager
    {
        private static LevelManager _level = null;
        public static LevelManager level => _level;

        private static AsyncOperation asyncLoadLevel;

        public static void LoadScene(int sceneIndex, bool allowSceneActivation)
        {
            asyncLoadLevel = SceneManager.LoadSceneAsync(sceneIndex);
            SetSceneActivation(allowSceneActivation);
        }

        public static void SetSceneActivation(bool allowSceneActivation)
        {
            asyncLoadLevel.allowSceneActivation = allowSceneActivation;
        }

        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeMethodLoad()
        {
            //SceneManager.LoadScene(0);
            LevelManager.On_LevelLoaded += SceneLoaded;
            Log(Type.Message, "Application Start.");
        }

        //[RuntimeInitializeOnLoadMethod]
        //static void OnSecondRuntimeMethodLoad()
        //{
        //    Debug.Log("SecondMethod After Scene is loaded and game is running.");
        //}

        /// <summary>
        /// Invoked by the LevelManager in the scene once the scene has finished loading
        /// </summary>
        /// <param name="levelManager">The LevelManager in the active scene</param>
        /// <param name="playerManager">The PlayerManager in the active scene</param>
        public static void SceneLoaded(string levelName, LevelManager levelManager)
        {
            if (levelName != string.Empty || levelManager != null)
            {
                Logger.Log(Type.Message, $"Level {levelName} successfully loaded.\n LevelManager={levelManager}.");
                _level = levelManager;
            }
            else
            {
                Logger.Log(Type.Error, $"Level {levelName} loading failed.\n LevelManager={levelManager}.");
                ExitApplication();
            }
        }

        public static void ExitApplication()
        {
            Logger.Log(Type.Message, "Application safely exited.");
            Application.Quit();
        }
    }
}