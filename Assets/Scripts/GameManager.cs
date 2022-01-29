using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
    public class GameManager : MonoBehaviour
    {
        public Scenes currentScene;
        
        // true : single      false : multi
        public static bool singlePlayer;
    #region Singleton
    public static GameManager instance;

        void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                
                ProfileControl.Load();
                ProfileControl.playerProfile.executionNumber++;
                ProfileControl.Save();
            }
        }

        #endregion

        public void ChangeScene(Scenes scene)
        {
            Time.timeScale = 1;
            currentScene = scene;
            SceneManager.LoadScene(scene.ToString());
        }

    }


