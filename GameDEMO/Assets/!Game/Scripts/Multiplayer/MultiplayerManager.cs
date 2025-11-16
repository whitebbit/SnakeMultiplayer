using System.Collections.Generic;
using _Game.Scripts.Multiplayer.Schemas;
using _Game.Scripts.Units;
using _Game.Scripts.Units.Enemy;
using _Game.Scripts.Units.Player;
using Colyseus;
using UnityEngine;

namespace _Game.Scripts.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        #region FIELDS SERIALIZED

        [SerializeField] private SnakeMovement playerAimPrefab;

        [SerializeField] private PlayerUnit playerPrefab;
        [SerializeField] private EnemyUnit enemyPrefab;

        #endregion

        #region FIELDS

        private const string GameRoomName = "state_handler";
        private ColyseusRoom<State> _room;
        private readonly Dictionary<string, EnemyUnit> _enemies = new();

        #endregion

        #region UNITY FUNCTIONS

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            InitializeClient();
            Connect();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            LeaveRoom();
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            LeaveRoom();
        }

        #endregion

        #region METHODS

        public void LeaveRoom() => _room?.Leave();

        public void SendMessage(string key, Dictionary<string, object> data)
        {
            _room.Send(key, data);
        }

        public void SendMessage(string key, string data)
        {
            _room.Send(key, data);
        }

        public string GetSessionID() => _room.SessionId;

        private async void Connect()
        {
            var data = new Dictionary<string, object>
            {
            };

            _room = await client.JoinOrCreate<State>(GameRoomName, data);
            _room.OnStateChange += OnStateChange;
        }

        private void OnStateChange(State state, bool isFirstState)
        {
            if (!isFirstState) return;

            state.players.ForEach((key, player) =>
            {
                if (key == _room.SessionId) CreatePlayer(player);
                else CreateEnemy(key, player);
            });

            _room.State.players.OnAdd += CreateEnemy;
            _room.State.players.OnRemove += RemoveEnemy;
        }

        private void CreatePlayer(Player player)
        {
            var position = player.position.ToVector3();
            var rotation = Quaternion.identity;
            var playerUnit = Instantiate(playerPrefab, position, rotation);
            var aim = Instantiate(playerAimPrefab, position, rotation);

            playerUnit.Initialize(player.d);
            aim.SetSpeed(playerUnit.Movement.MoveSpeed);

            if (!playerUnit.TryGetComponent(out PlayerStateTransmitter stateTransmitter)) return;

            stateTransmitter.SetMovement(aim);
            playerUnit.Controller.Initialize(player, playerUnit, aim, stateTransmitter);
        }

        private void CreateEnemy(string key, Player player)
        {
            var position = player.position.ToVector3();
            var enemyUnit = Instantiate(enemyPrefab, position, Quaternion.identity);

            enemyUnit.Initialize(player.d);

            enemyUnit.Controller.Initialize(player, enemyUnit);

            _enemies.Add(key, enemyUnit);
        }

        private void RemoveEnemy(string key, Player player)
        {
            if (!_enemies.Remove(key, out var enemy)) return;

            enemy.Destroy();
        }

        #endregion
    }
}