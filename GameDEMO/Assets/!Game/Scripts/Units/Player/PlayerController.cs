using System.Collections.Generic;
using _Game.Scripts.Multiplayer.Schemas;
using _Game.Scripts.Units.Interfaces;
using Colyseus.Schema;
using UnityEngine;

namespace _Game.Scripts.Units.Player
{
    public class PlayerController : MonoBehaviour, IUnitController
    {
        [SerializeField] private Transform cursor;

        private PlayerStateTransmitter _stateTransmitter;
        private SnakeMovement _movement;
        private SnakeMovement _aim;
        private Camera _camera;
        private global::Player _player;
        private Plane _plane;
        private PlayerUnit _unit;

        public void Initialize(global::Player player, PlayerUnit unit, SnakeMovement aim,
            PlayerStateTransmitter stateTransmitter)
        {
            _player = player;
            _stateTransmitter = stateTransmitter;
            _unit = unit;
            _movement = unit.Movement;
            _aim = aim;

            _plane = new Plane(Vector3.up, Vector3.zero);
            _camera = Camera.main;

            _player.OnChange += OnChange;
        }

        private void Update()
        {
            if (!_movement || !_aim) return;

            if (Input.GetMouseButton(0))
            {
                MoveCursor();
                _aim.LerpRotation(cursor.position);
            }

            _aim.Move();
            _aim.Rotate();

            _movement.Move();
            _movement.Rotate();

            _stateTransmitter.SendTransform();
        }

        private void OnDestroy()
        {
            _player.OnChange -= OnChange;
        }

        private void MoveCursor()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            _plane.Raycast(ray, out var distance);
            var point = ray.GetPoint(distance);
            cursor.position = point;
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

            _movement.SetRotation(position);
        }
    }
}