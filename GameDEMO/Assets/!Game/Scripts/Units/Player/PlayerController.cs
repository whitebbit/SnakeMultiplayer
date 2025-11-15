using System;
using _Game.Scripts.Multiplayer;
using UnityEngine;

namespace _Game.Scripts.Units.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform cursor;

        private PlayerStateTransmitter _stateTransmitter;
        private SnakeMovement _movement;
        private Camera _camera;
        private Plane _plane;

        public void Initialize(SnakeMovement movement, PlayerStateTransmitter stateTransmitter)
        {
            _stateTransmitter = stateTransmitter;
            _movement = movement;
            _plane = new Plane(Vector3.up, Vector3.zero);
            _camera = Camera.main;
        }

        private void Update()
        {
            var button = Input.GetMouseButton(0);
            cursor.gameObject.SetActive(button);

            if (button)
            {
                MoveCursor();
                _movement.LookAt(cursor);
            }

            _movement.Rotate();
            _movement.Move();
            _stateTransmitter.SendTransform();
        }

        private void MoveCursor()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            _plane.Raycast(ray, out var distance);
            var point = ray.GetPoint(distance);
            cursor.position = point;
        }
    }
}