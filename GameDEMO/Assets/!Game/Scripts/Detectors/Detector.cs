using System;
using _Game.Scripts.Map;
using _Game.Scripts.Map.Interfaces;
using UnityEngine;

namespace _Game.Scripts.Detectors
{
    public class Detector : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        [SerializeField] private float radius;

        #endregion

        #region FIELDS

        private Transform _point;
        private readonly Collider[] _results = new Collider[32];

        #endregion

        #region UNITY FUNCTIONS

        private void FixedUpdate()
        {
            DetectCollisions();
        }

        #endregion

        #region METHODS

        public void Initialize(Transform point)
        {
            _point = point;
        }

        private void DetectCollisions()
        {
            var size = Physics.OverlapSphereNonAlloc(_point.position, radius, _results);

            for (var i = 0; i < size; i++)
            {
                if (_results[i].TryGetComponent(out ICollectable obj)) obj.Collect();
            }
        }

        #endregion

        private void OnDrawGizmos()
        {
            if (!_point) return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_point.position, radius);
        }
    }
}