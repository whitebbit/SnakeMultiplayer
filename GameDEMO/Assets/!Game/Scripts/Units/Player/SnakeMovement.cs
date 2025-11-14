using UnityEngine;

namespace _Game.Scripts.Units.Player
{
    public class SnakeMovement : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        [SerializeField] private float moveSpeed = 2;
        [SerializeField] private float rotationSpeed = 90;
        [SerializeField] private Transform head;

        #endregion

        #region FIELDS

        public Transform Head => head;
        public float MoveSpeed => moveSpeed;

        private Vector3 _targetDirection;

        #endregion

        #region UNITY FUNCTIONS

        #endregion

        #region METHODS

        public void Move()
        {
            transform.position += head.forward * (Time.deltaTime * moveSpeed);
        }

        public void Rotate()
        {
            var targetRotation = Quaternion.LookRotation(_targetDirection);
            head.rotation =
                Quaternion.RotateTowards(head.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }


        public void LookAt(Transform target)
        {
            _targetDirection = target.position - head.position;
        }

        #endregion
    }
}