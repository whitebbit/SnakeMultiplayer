using System.Collections.Generic;
using _Game.Scripts.Multiplayer.Schemas;
using _Game.Scripts.Units.Interfaces;
using Colyseus.Schema;
using UnityEngine;

namespace _Game.Scripts.Units.Enemy
{
    public class EnemyController : MonoBehaviour, IUnitController
    {
        #region FIELDS SERIALIZED

        #endregion

        #region FIELDS

        private global::Player _player;
        private EnemyUnit _unit;
        private SnakeMovement _snakeMovement;
        
        #endregion

        #region UNITY FUNCTIONS

        private void Update()
        {
            _snakeMovement.Move();
        }
        
        private void OnDestroy()
        {
            _player.OnChange -= OnChange;
        }

        #endregion

        #region METHODS

        public void Initialize(global::Player player, EnemyUnit unit)
        {
            _player = player;
            _unit = unit;
            _snakeMovement = unit.Movement;
            
            _player.OnChange += OnChange;
        }

        public void OnChange(List<DataChange> changes)
        {
            var position = _unit.transform.position;

            foreach (var change in changes)
            {
                switch (change.Field)
                {
                    case "position":
                        var positionValue = (Vector3Schema)change.Value;
                        position = positionValue.ToVector3();
                        break;
                    case "d":
                        _unit.SetDetailCount((byte)change.Value);
                        break;
                    default:
                        Debug.LogWarning($"{change.Field} not supported");
                        break;
                }
            }
            
            _snakeMovement.SetRotation(position);
        }

        #endregion
    }
}