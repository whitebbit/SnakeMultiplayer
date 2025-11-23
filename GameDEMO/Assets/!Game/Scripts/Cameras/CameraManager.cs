using System;
using UnityEngine;

namespace _Game.Scripts.Cameras
{
    public class CameraManager : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        #endregion

        #region FIELDS

        private Transform _camera;

        #endregion

        #region UNITY FUNCTIONS

        private void Start()
        {
            if (!Camera.main) return;

            _camera = Camera.main.transform;
            _camera.position = transform.position;
        }

        private void Update()
        {
            _camera.transform.position =
                Vector3.Lerp(_camera.transform.position, transform.position, Time.deltaTime * 5);
        }

        #endregion

        #region METHODS

        #endregion
    }
}