using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Units
{
    public class SnakeTail : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        [SerializeField] private Transform detailPrefab;
        [SerializeField] private float detailDistance = 1;

        #endregion

        #region FIELDS

        private readonly List<Transform> _details = new();
        private readonly List<Vector3> _positionHistory = new();

        private float _speed;
        private Transform _head;
        
        #endregion

        #region UNITY FUNCTIONS

        private void Update()
        {
            UpdatePositionHistory(out var distance);
            UpdateTailPosition(distance);
        }

        #endregion

        #region METHODS

        public void Initialize(Transform head, float speed, int detailCount)
        {
            _head = head;
            _speed = speed;
            
            _details.Add(transform);
            
            _positionHistory.Add(_head.position);
            _positionHistory.Add(transform.position);

            SetDetailCount(detailCount);
        }

        private void SetDetailCount(int detailCount)
        {
            if (detailCount == _details.Count + 1) return;

            var diff = (_details.Count - 1) - detailCount;

            if (diff < 1)
            {
                for (var i = 0; i < -diff; i++)
                {
                    AddDetail();
                }
            }
            else
            {
                for (var i = 0; i < diff; i++)
                {
                    RemoveDetail();
                }
            }
        }

        private void AddDetail()
        {
            var position = _details[^1].position;
            var detail = Instantiate(detailPrefab, position, Quaternion.identity);

            _details.Insert(0, detail);
            _positionHistory.Add(position);
        }

        private void RemoveDetail()
        {
            if (_details.Count <= 1) return;

            var detail = _details[0];
            _details.Remove(detail);
            Destroy(detail.gameObject);
            _positionHistory.RemoveAt(_positionHistory.Count - 1);
        }
        
        private void UpdateTailPosition(float distance)
        {
            for (var i = 0; i < _details.Count; i++)
            {
                _details[i].position =
                    Vector3.Lerp(_positionHistory[i + 1], _positionHistory[i], distance / detailDistance);
            }
        }

        private void UpdatePositionHistory(out float distance)
        {
            var position = _positionHistory[0];
            distance = (_head.position - position).magnitude;

            while (distance > detailDistance)
            {
                var direction = (_head.position - position).normalized;

                _positionHistory.Insert(0, position + direction * detailDistance);
                _positionHistory.RemoveAt(_positionHistory.Count - 1);

                distance -= detailDistance;
            }
        }

        public void Destroy()
        {
            foreach (var detail in _details)
            {
                Destroy(detail.gameObject);
            }
        }

        #endregion
    }
}