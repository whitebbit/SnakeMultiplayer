using System;
using UnityEngine;

namespace _Game.Scripts.Extensions
{
    public class LookAtCamera : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        #endregion

        #region FIELDS

        private Transform _camera;

        #endregion

        #region UNITY FUNCTIONS

        private void Start()
        {
            if (Camera.main) _camera = Camera.main.transform;
        }

        private void Update()
        {
            if (_camera) transform.LookAt(_camera);
        }

        #endregion

        #region METHODS

        #endregion
    }
}