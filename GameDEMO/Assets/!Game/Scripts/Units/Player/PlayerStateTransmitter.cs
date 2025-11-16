using System;
using System.Collections.Generic;
using _Game.Scripts.Multiplayer;
using UnityEngine;

namespace _Game.Scripts.Units.Player
{
    public class PlayerStateTransmitter : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        #endregion

        #region FIELDS

        private SnakeMovement _movement;
        private MultiplayerManager _multiplayerManager;

        #endregion

        #region UNITY FUNCTIONS

        private void Start()
        {
            _multiplayerManager = MultiplayerManager.Instance;
        }

        #endregion

        #region METHODS

        public void SetMovement(SnakeMovement movement) => _movement = movement;

        public void SendTransform(Vector3 customPosition = default)
        {
            _movement.GetMoveInfo(out var position);

            var pos = customPosition == default ? position : customPosition;

            var data = new Dictionary<string, object>
            {
                {
                    "pos", new Dictionary<string, float>
                    {
                        { "x", pos.x },
                        { "y", pos.y },
                        { "z", pos.z }
                    }
                },
            };

            _multiplayerManager.SendMessage("move", data);
        }

        #endregion
    }
}