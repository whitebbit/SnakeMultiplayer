using _Game.Scripts.Units.Interfaces;
using _Game.Scripts.Units.Player;
using UnityEngine;

namespace _Game.Scripts.Units
{
    public abstract class Unit<TController> : MonoBehaviour where TController : IUnitController
    {
        #region FIELDS SERIALIZED

        [SerializeField] private SnakeMovement movement;
        [SerializeField] private SnakeTail tailPrefab;

        #endregion

        #region FIELDS

        public abstract TController Controller { get; protected set; }
        public SnakeMovement Movement => movement;

        private SnakeTail _tail;

        #endregion

        #region UNITY FUNCTIONS

        #endregion

        #region METHODS

        public virtual void Initialize(int detailCount)
        {
            _tail = Instantiate(tailPrefab, transform.position, Quaternion.identity);
            _tail.Initialize(movement.Head, detailCount);
        }

        public virtual void Destroy()
        {
            _tail.Destroy();
            Destroy(gameObject);
        }

        public void SetDetailCount(int detailCount) => _tail.SetDetailCount(detailCount);
        
        #endregion
    }
}