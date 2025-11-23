using System;
using UnityEngine;

namespace _Game.Scripts.Units
{
    public class PlayerSettings : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        #endregion

        #region FIELDS

        public static PlayerSettings Instance { get; private set; }

        public string Nickname { get; private set; }

        #endregion

        #region UNITY FUNCTIONS

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        #endregion

        #region METHODS

        public void SetNickname(string nickname) => Nickname = nickname;

        #endregion
    }
}