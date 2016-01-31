using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.View
{
    public class MenuManager : MonoBehaviour
    {
        #region Constants

        public const string SCENE_MENU = "MainMenu";
        public const string SCENE_PLAY = "Play";
        public const string SCENE_CREDITS = "Credits";

        #endregion

        public void OnPlayButton()
        {

            SceneManager.LoadScene(SCENE_PLAY);

        }
        public void OnCredists()
        {

            SceneManager.LoadScene(SCENE_CREDITS);

        }
        public void OnExit()
        {
            Application.Quit();

        }


    }
}
