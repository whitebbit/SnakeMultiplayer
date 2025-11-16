using _Game.Scripts.Units.Skins.Enums;
using UnityEngine;

namespace _Game.Scripts.Units
{
    public class SnakePart : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        [SerializeField] private SnakePartType type;
        [SerializeField] private MeshRenderer[] renderers;

        #endregion

        #region FIELDS

        public SnakePartType Type => type;

        #endregion

        #region UNITY FUNCTIONS

        #endregion

        #region METHODS

        public void SetMaterial(Material material)
        {
            foreach (var renderer in renderers)
            {
                renderer.material = material;
            }
        }

        #endregion
    }
}