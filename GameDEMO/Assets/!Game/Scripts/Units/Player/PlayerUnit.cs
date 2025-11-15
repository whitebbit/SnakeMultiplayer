using System;
using UnityEngine;

namespace _Game.Scripts.Units.Player
{
    public class PlayerUnit : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        [SerializeField] private SnakeMovement movement;
        [SerializeField] private SnakeTail tailPrefab;

        #endregion

        #region FIELDS

        private SnakeTail _tail;
        
        #endregion

        #region UNITY FUNCTIONS

        #endregion

        #region METHODS

        public void Initialize(int detailCount)
        {
            _tail = Instantiate(tailPrefab, transform.position, Quaternion.identity);
            _tail.Initialize(movement.Head, detailCount);
        }

        public void Destroy()
        {
            _tail.Destroy();
            Destroy(gameObject);
        }

        #endregion
    }
}