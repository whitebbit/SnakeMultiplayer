using UnityEngine;

namespace _Game.Scripts.Units
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
            if (head)
                transform.position += head.forward * (Time.deltaTime * moveSpeed);
            else
                transform.position += transform.forward * (Time.deltaTime * moveSpeed);
        }

        public void Rotate()
        {
            var targetRotation = Quaternion.LookRotation(_targetDirection);
            if (head)
                head.rotation =
                    Quaternion.RotateTowards(head.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            else
                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        public void LerpRotation(Vector3 pointToLook)
        {
            if (head)
                _targetDirection = pointToLook - head.position;
            else
                _targetDirection = pointToLook - transform.position;
        }

        public void SetRotation(Vector3 pointToLook)
        {
            if (head)
            {
                _targetDirection = pointToLook - head.position;
                head.LookAt(pointToLook);
            }
            else
            {
                _targetDirection = pointToLook - transform.position;
                transform.LookAt(pointToLook);
            }
        }

        public void GetMoveInfo(out Vector3 position)
        {
            position = transform.position;
        }

        public void SetSpeed(float speed) => moveSpeed = speed;

        #endregion
    }
}