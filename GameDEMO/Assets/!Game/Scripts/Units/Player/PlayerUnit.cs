using System;
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
        
        #endregion
    }
}