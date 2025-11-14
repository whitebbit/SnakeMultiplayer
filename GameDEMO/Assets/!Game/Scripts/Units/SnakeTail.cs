using System;
using System.Collections.Generic;
using _Game.Scripts.Units.Player;
using UnityEngine;

namespace _Game.Scripts.Units
{
    public class SnakeTail : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        [SerializeField] private PlayerMovement movement;
        [SerializeField] private List<Transform> details = new();
        [SerializeField] private float detailDistance = 1;

        #endregion

        #region FIELDS

        private readonly List<Vector3> _positionHistory = new();

        #endregion

        #region UNITY FUNCTIONS

        private void Awake()
        {
            _positionHistory.Add(movement.Head.position);
            foreach (var detail in details)
            {
                _positionHistory.Add(detail.position);
            }
        }

        private void Update()
        {
            UpdatePositionHistory(out var distance);
            UpdateTailPosition(distance);
        }

        private void UpdateTailPosition(float distance)
        {
            for (var i = 0; i < details.Count; i++)
            {
                details[i].position =
                    Vector3.Lerp(_positionHistory[i + 1], _positionHistory[i], distance / detailDistance);

                var direction = (_positionHistory[i] - _positionHistory[i + 1]).normalized;

                details[i].position += direction * (Time.deltaTime * movement.MoveSpeed);
            }
        }

        private void UpdatePositionHistory(out float distance)
        {
            var position = _positionHistory[0];
            distance = (movement.Head.position - position).magnitude;

            while (distance > detailDistance)
            {
                var direction = (movement.Head.position - position).normalized;

                _positionHistory.Insert(0, position + direction * detailDistance);
                _positionHistory.RemoveAt(_positionHistory.Count - 1);

                distance -= detailDistance;
            }
        }

        #endregion

        #region METHODS

        #endregion
    }
}