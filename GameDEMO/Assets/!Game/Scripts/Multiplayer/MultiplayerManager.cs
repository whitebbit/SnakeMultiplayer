using System.Collections.Generic;
using _Game.Scripts.Multiplayer.Schemas;
using _Game.Scripts.Units.Player;
using Colyseus;
using UnityEngine;

namespace _Game.Scripts.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        #region FIELDS SERIALIZED

        [SerializeField] private PlayerController controllerPrefab;
        [SerializeField] private PlayerUnit playerPrefab;

        #endregion

        #region FIELDS

        private const string GameRoomName = "state_handler";
        private ColyseusRoom<State> _room;

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
            var playerUnit = Instantiate(playerPrefab, position, Quaternion.identity);
            var controller = Instantiate(controllerPrefab);

            playerUnit.Initialize(player.d);

            if (playerUnit.TryGetComponent(out SnakeMovement movement) &&
                playerUnit.TryGetComponent(out PlayerStateTransmitter stateTransmitter))
                controller.Initialize(movement, stateTransmitter);
        }

        private void CreateEnemy(string key, Player player)
        {
        }

        private void RemoveEnemy(string key, Player player)
        {
        }

        #endregion
    }
}