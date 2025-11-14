using System;
using UnityEngine;

namespace _Game.Scripts.Units.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform cursor;
        
        private SnakeMovement _movement;
        private Camera _camera;
        private Plane _plane;

        public void Initialize(SnakeMovement movement)
        {
            _movement = movement;
            _plane = new Plane(Vector3.up, Vector3.zero);
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                cursor.gameObject.SetActive(true);
            }
            
            if (Input.GetMouseButton(0))
            {
                MoveCursor();
                _movement.LookAt(cursor);
            }

            if (Input.GetMouseButtonUp(0))
            {
                cursor.gameObject.SetActive(false);
            }
            
            _movement.Rotate();
            _movement.Move();
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