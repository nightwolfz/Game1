using UnityEngine;

namespace Assets.Scripts
{
    public class PauseMenu : MonoBehaviour
    {

        public enum ButtonAction
        {
            ExitGame, UnPause
        };

        public ButtonAction Action;

        void OnMouseDown()
        {

            if (Action == ButtonAction.ExitGame)
            {
                Application.Quit();
            }

            if (Action == ButtonAction.UnPause)
            {
                SceneManager.Instance.PauseGame();
            }
        }

    }
}
