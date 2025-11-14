using System;
using _Game.Scripts.Units.Player;
using UnityEngine;

namespace _Game.Scripts
{
    public class Test : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        [SerializeField] private int detailCount;
        [SerializeField] private PlayerController controllerPrefab;
        [SerializeField] private PlayerUnit playerPrefab;

        #endregion

        #region FIELDS

        private PlayerUnit _playerUnit;
        private PlayerController _controller;

        #endregion

        #region UNITY FUNCTIONS

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Return)) return;

            if (_playerUnit) _playerUnit.Destroy();
            if (_controller) Destroy(_controller.gameObject);

            _playerUnit = Instantiate(playerPrefab);
            _controller = Instantiate(controllerPrefab);

            _playerUnit.Initialize(detailCount);
            _controller.Initialize(_playerUnit.GetComponent<SnakeMovement>());
        }

        #endregion

        #region METHODS

        #endregion
    }
}