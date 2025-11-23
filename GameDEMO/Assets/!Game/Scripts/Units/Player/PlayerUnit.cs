using System;
using _Game.Scripts.Extensions;
using _Game.Scripts.Multiplayer;
using _Game.Scripts.Units.Skins;
using UnityEngine;

namespace _Game.Scripts.Units.Player
{
    public class PlayerUnit : Unit<PlayerController>
    {
        #region FIELDS SERIALIZED

        [SerializeField] private PlayerController controllerPrefab;

        #endregion

        #region FIELDS

        public override PlayerController Controller { get; protected set; }

        #endregion

        #region UNITY FUNCTIONS

        private void Awake()
        {
            Controller = Instantiate(controllerPrefab, Vector3.zero, Quaternion.identity);
        }

        #endregion

        #region METHODS

        public override void Initialize(string clientId, global::Player player, UnitSkin unitSkin)
        {
            gameObject.SetLayer("Player");

            base.Initialize(clientId, player, unitSkin);

            Tail.SetLayer("Player");
        }

        public override void Destroy()
        {
            var detailPositions = Tail.GetDetailPositions();
            var gameOverData = new GameOverData
            {
                id = ClientId,
                dP = detailPositions
            };

            var data = JsonUtility.ToJson(gameOverData);

            MultiplayerManager.Instance.SendMessage("gameOver", data);

            base.Destroy();
        }

        #endregion
    }

    [Serializable]
    public struct GameOverData
    {
        public string id;
        public Vector2[] dP;
    }
}