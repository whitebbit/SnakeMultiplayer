using System.Collections.Generic;
using _Game.Scripts.Map.Interfaces;
using _Game.Scripts.Multiplayer;
using _Game.Scripts.Multiplayer.Schemas;
using Colyseus.Schema;
using UnityEngine;

namespace _Game.Scripts.Map
{
    public class Apple : MonoBehaviour, ICollectable
    {
        #region FIELDS SERIALIZED

        #endregion

        #region FIELDS

        private AppleSchema _appleSchema;

        #endregion

        #region UNITY FUNCTIONS

        #endregion

        #region METHODS

        public void Initialize(AppleSchema appleSchema)
        {
            _appleSchema = appleSchema;
            _appleSchema.OnChange += OnChange;
        }

        public void Destroy()
        {
            if (_appleSchema != null)
                _appleSchema.OnChange -= OnChange;

            Destroy(gameObject);
        }

        #endregion

        public void Collect()
        {
            SendCollectToServer();
        }

        private void SendCollectToServer()
        {
            gameObject.SetActive(false);

            var data = new Dictionary<string, object>
            {
                { "id", _appleSchema.id },
            };

            MultiplayerManager.Instance.SendMessage("collect", data);
        }

        private void OnChange(List<DataChange> changes)
        {
            var position = transform.position;

            foreach (var change in changes)
            {
                switch (change.Field)
                {
                    case "position":
                        var positionValue = (Vector2Schema)change.Value;
                        position = positionValue.ToVector3();
                        break;
                }
            }

            transform.position = position;
            gameObject.SetActive(true);
        }
    }
}