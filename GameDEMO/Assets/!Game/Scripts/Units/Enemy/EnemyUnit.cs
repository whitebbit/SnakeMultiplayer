using System;

namespace _Game.Scripts.Units.Enemy
{
    public class EnemyUnit : Unit<EnemyController>
    {
        #region FIELDS SERIALIZED

        #endregion

        #region FIELDS
        
        public override EnemyController Controller { get; protected set; } 

        #endregion

        #region UNITY FUNCTIONS

        private void Awake()
        {
            Controller = GetComponent<EnemyController>();
        }

        #endregion

        #region METHODS

        #endregion
    }
}