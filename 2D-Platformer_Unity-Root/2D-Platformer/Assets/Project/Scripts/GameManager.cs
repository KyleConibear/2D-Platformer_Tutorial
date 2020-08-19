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
        private const string levelName = "Level-1_Scene";

        private static LevelManager _level = null;
        public static LevelManager level => _level;

        /// <summary>
        /// Automatically runs when the application starts
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        private static void LoadGameLevel()
        {
            LevelManager.On_LevelLoaded += LevelLoaded;
            AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(levelName);
        }

        /// <summary>
        /// Invoked by the LevelManager in the scene once the scene has finished loading
        /// </summary>
        /// <param name="levelManager">The LevelManager in the active scene</param>
        /// <param name="playerManager">The PlayerManager in the active scene</param>
        private static void LevelLoaded(string levelName, LevelManager levelManager)
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