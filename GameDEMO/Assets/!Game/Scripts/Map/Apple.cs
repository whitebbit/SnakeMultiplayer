using UnityEngine;

namespace _Game.Scripts.Map
{
    public class Apple : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        #endregion

        #region FIELDS

        private Vector2Schema _vector2Schema;

        #endregion

        #region UNITY FUNCTIONS

        #endregion

        #region METHODS

        public void Initialize(Vector2Schema vector2Schema)
        {
            _vector2Schema = vector2Schema;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        #endregion
    }
}