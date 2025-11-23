using System;
using System.Collections.Generic;
using _Game.Scripts.Extensions;
using _Game.Scripts.Units.Skins;
using UnityEngine;

namespace _Game.Scripts.Units
{
    public class SnakeTail : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        [SerializeField] private SnakePart detailPrefab;
        [SerializeField] private float detailDistance = 1;

        #endregion

        #region FIELDS

        private readonly List<SnakePart> _details = new();
        private readonly List<Vector3> _positionHistory = new();
        private readonly List<Quaternion> _rotationHistory = new();

        private SnakePart _head;
        private UnitSkinLoader _skinLoader;

        private string _layer;

        #endregion

        #region UNITY FUNCTIONS

        private void Update()
        {
            UpdateTransformHistory(out var distance);
            UpdateTailTransform(distance);
        }

        #endregion

        #region METHODS

        public void Initialize(SnakePart head, int detailCount, UnitSkinLoader skinLoader)
        {
            _skinLoader = skinLoader;
            _head = head;

            if (TryGetComponent(out SnakePart part))
            {
                skinLoader.AddSkinPart(part);
                _details.Add(part);
            }

            _positionHistory.Add(_head.transform.position);
            _positionHistory.Add(transform.position);

            _rotationHistory.Add(_head.transform.rotation);
            _rotationHistory.Add(transform.rotation);

            SetDetailCount(detailCount);
        }

        public void SetLayer(string layer)
        {
            _layer = layer;

            gameObject.SetLayer(layer);
            foreach (var detail in _details)
                detail.gameObject.SetLayer(layer);
        }

        public void SetDetailCount(int detailCount)
        {
            if (detailCount == _details.Count - 1) return;

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
            var position = _details[^1].transform.position;
            var rotation = _details[^1].transform.rotation;
            var detail = Instantiate(detailPrefab, position, rotation);

            _details.Insert(0, detail);

            _positionHistory.Add(position);
            _rotationHistory.Add(rotation);

            _skinLoader.AddSkinPart(detail);

            if (string.IsNullOrEmpty(_layer) == false)
                detail.gameObject.SetLayer(_layer);
        }

        private void RemoveDetail()
        {
            if (_details.Count <= 1) return;

            var detail = _details[0];

            _details.Remove(detail);
            _skinLoader.RemoveSkinPart(detail);

            Destroy(detail.gameObject);

            _positionHistory.RemoveAt(_positionHistory.Count - 1);
            _rotationHistory.RemoveAt(_rotationHistory.Count - 1);
        }

        private void UpdateTailTransform(float distance)
        {
            for (var i = 0; i < _details.Count; i++)
            {
                var delta = distance / detailDistance;
                var detail = _details[i];

                detail.transform.position = Vector3.Lerp(_positionHistory[i + 1], _positionHistory[i], delta);
                detail.transform.rotation = Quaternion.Lerp(_rotationHistory[i + 1], _rotationHistory[i], delta);
            }
        }

        private void UpdateTransformHistory(out float distance)
        {
            var position = _positionHistory[0];
            distance = (_head.transform.position - position).magnitude;

            while (distance > detailDistance)
            {
                var direction = (_head.transform.position - position).normalized;

                _positionHistory.Insert(0, position + direction * detailDistance);
                _positionHistory.RemoveAt(_positionHistory.Count - 1);

                _rotationHistory.Insert(0, _head.transform.rotation);
                _rotationHistory.RemoveAt(_rotationHistory.Count - 1);

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

        public Vector2[] GetDetailPositions()
        {
            var positions = new Vector2[_details.Count];

            for (var i = 0; i < _details.Count; i++)
            {
                positions[i] = new Vector2(_details[i].transform.position.x, _details[i].transform.position.z);
            }

            return positions;
        }

        #endregion
    }

    
}