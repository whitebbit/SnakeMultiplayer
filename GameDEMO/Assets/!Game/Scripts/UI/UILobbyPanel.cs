using _Game.Scripts.Units;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts.UI
{
    public class UILobbyPanel : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        #endregion

        #region FIELDS

        #endregion

        #region UNITY FUNCTIONS

        #endregion

        #region METHODS

        public void InputNickname(string nickname)
        {
            PlayerSettings.Instance.SetNickname(nickname);
        }

        public void OnClickConnect()
        {
            if (string.IsNullOrEmpty(PlayerSettings.Instance.Nickname)) return;

            SceneManager.LoadScene("Game");
        }

        #endregion
    }
}