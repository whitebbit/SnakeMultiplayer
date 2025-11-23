using _Game.Scripts.Multiplayer;
using _Game.Scripts.Units.Interfaces;
using _Game.Scripts.Units.Player;
using _Game.Scripts.Units.Skins;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Units
{
    public abstract class Unit<TController> : MonoBehaviour where TController : IUnitController
    {
        #region FIELDS SERIALIZED

        [SerializeField] private SnakeMovement movement;
        [SerializeField] private SnakeTail tailPrefab;
        [SerializeField] private UnitSkinLoader skinLoader;
        [SerializeField] private TMP_Text nicknameText;

        #endregion

        #region FIELDS

        public abstract TController Controller { get; protected set; }
        public SnakeMovement Movement => movement;

        protected SnakeTail Tail;
        public string ClientId { get; private set; }

        private global::Player _player;

        #endregion

        #region UNITY FUNCTIONS

        #endregion

        #region METHODS

        public virtual void Initialize(string clientId, global::Player player, UnitSkin unitSkin)
        {
            ClientId = clientId;
            _player = player;

            Tail = Instantiate(tailPrefab, transform.position, Quaternion.identity);
            Tail.Initialize(movement.Head, _player.d, skinLoader);

            skinLoader.LoadSkin(unitSkin);

            nicknameText.text = player.nickname;
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