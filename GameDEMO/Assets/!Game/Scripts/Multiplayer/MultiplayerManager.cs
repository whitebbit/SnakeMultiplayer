using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Map;
using _Game.Scripts.Multiplayer.Schemas;
using _Game.Scripts.Units;
using _Game.Scripts.Units.Enemy;
using _Game.Scripts.Units.Player;
using _Game.Scripts.Units.Skins;
using Colyseus;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        #region FIELDS SERIALIZED

        [SerializeField] private SnakeMovement playerAimPrefab;
        [SerializeField] private PlayerUnit playerPrefab;
        [SerializeField] private EnemyUnit enemyPrefab;
        [SerializeField] private Apple applePrefab;

        [SerializeField] private TMP_Text leaderboardText;

        [SerializeField] private UnitSkin[] skins;

        #endregion

        #region FIELDS

        private const string GameRoomName = "state_handler";
        private ColyseusRoom<State> _room;

        private readonly Dictionary<string, EnemyUnit> _enemies = new();
        private readonly Dictionary<AppleSchema, Apple> _apples = new();
        private readonly Dictionary<string, LeaderboardPair> _leaderboard = new();

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
                { "sC", skins.Length },
                { "nickname", PlayerSettings.Instance.Nickname },
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

            _room.State.apples.ForEach(CreateApple);
            _room.State.apples.OnAdd += (_, apple) => CreateApple(apple);
            _room.State.apples.OnRemove += RemoveApple;
        }


        private void CreatePlayer(Player player)
        {
            var position = player.position.ToVector3();
            var rotation = Quaternion.identity;
            var playerUnit = Instantiate(playerPrefab, position, rotation);
            var aim = Instantiate(playerAimPrefab, position, rotation);

            playerUnit.Initialize(_room.SessionId, player, skins[player.sI]);
            aim.SetSpeed(playerUnit.Movement.MoveSpeed);

            var stateTransmitter = playerUnit.GetComponent<PlayerStateTransmitter>();

            stateTransmitter.SetMovement(aim);
            playerUnit.Controller.Initialize(player, playerUnit, aim, stateTransmitter);

            AddLeader(_room.SessionId, player);
        }

        private void CreateEnemy(string key, Player player)
        {
            var position = player.position.ToVector3();
            var enemyUnit = Instantiate(enemyPrefab, position, Quaternion.identity);

            enemyUnit.Initialize(key, player, skins[player.sI]);

            enemyUnit.Controller.Initialize(player, enemyUnit);

            _enemies.Add(key, enemyUnit);

            AddLeader(key, player);
        }

        private void RemoveEnemy(string key, Player player)
        {
            RemoveLeader(key);

            if (!_enemies.Remove(key, out var enemy)) return;

            enemy.Destroy();
        }

        private void CreateApple(AppleSchema appleSchema)
        {
            var position = appleSchema.position.ToVector3();
            var apple = Instantiate(applePrefab, position, Quaternion.identity);

            apple.Initialize(appleSchema);
            _apples.Add(appleSchema, apple);
        }

        private void RemoveApple(int key, AppleSchema value)
        {
            if (_apples.Remove(value, out var apple)) apple.Destroy();
        }

        public void UpdateScore(string sessionID, int score)
        {
            if (!_leaderboard.TryGetValue(sessionID, out var leader)) return;
            
            leader.Score = score;
            UpdateLeaderboard();
        }

        private void AddLeader(string key, Player player)
        {
            _leaderboard.TryAdd(key, new LeaderboardPair { Nickname = player.nickname, Score = player.score });
            UpdateLeaderboard();
        }

        private void RemoveLeader(string key)
        {
            _leaderboard.Remove(key);
            UpdateLeaderboard();
        }

        private void UpdateLeaderboard()
        {
            var topCount = Math.Clamp(_leaderboard.Count, 0, 5);
            var top5 = _leaderboard.OrderByDescending(x => x.Value.Score).Take(topCount);

            var text = "";
            var index = 1;

            foreach (var leader in top5)
            {
                text += $"{index}. {leader.Value.Nickname}: {leader.Value.Score}\n";
                index++;
            }

            leaderboardText.text = text;
        }

        #endregion
    }

    public class LeaderboardPair
    {
        public string Nickname;
        public int Score;
    }
}