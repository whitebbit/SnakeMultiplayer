using _Game.Scripts.Units.Interfaces;
using _Game.Scripts.Units.Player;
using _Game.Scripts.Units.Skins;
using UnityEngine;

namespace _Game.Scripts.Units
{
    public abstract class Unit<TController> : MonoBehaviour where TController : IUnitController
    {
        #region FIELDS SERIALIZED

        [SerializeField] private SnakeMovement movement;
        [SerializeField] private SnakeTail tailPrefab;
        [SerializeField] private UnitSkinLoader skinLoader;

        #endregion

        #region FIELDS

        public abstract TController Controller { get; protected set; }
        public SnakeMovement Movement => movement;

        protected SnakeTail Tail;

        #endregion

        #region UNITY FUNCTIONS

        #endregion

        #region METHODS

        public virtual void Initialize(int detailCount, UnitSkin unitSkin)
        {
            Tail = Instantiate(tailPrefab, transform.position, Quaternion.identity);
            Tail.Initialize(movement.Head, detailCount, skinLoader);

            skinLoader.LoadSkin(unitSkin);
        }

        public virtual void Destroy()
        {
            Tail.Destroy();
            Destroy(gameObject);
        }

        public void SetDetailCount(int detailCount) => Tail.SetDetailCount(detailCount);

        #endregion
    }
}