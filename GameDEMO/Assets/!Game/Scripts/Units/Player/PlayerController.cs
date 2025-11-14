using System;
using UnityEngine;

namespace _Game.Scripts.Units.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private Transform cursor;
        
        private Camera _camera;
        private Plane _plane;

        private void Awake()
        {
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
                movement.LookAt(cursor);
            }

            if (Input.GetMouseButtonUp(0))
            {
                cursor.gameObject.SetActive(false);
            }
            
            movement.Rotate();
            movement.Move();
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