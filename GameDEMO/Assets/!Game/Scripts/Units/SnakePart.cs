using _Game.Scripts.Units.Skins.Enums;
using UnityEngine;

namespace _Game.Scripts.Units
{
    public class SnakePart : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        [SerializeField] private SnakePartType type;
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private MeshRenderer[] renderers;

        #endregion

        #region FIELDS

        public SnakePartType Type => type;
        private Material _material;

        #endregion

        #region UNITY FUNCTIONS

        private void OnDestroy()
        {
            var obj = Instantiate(particle, transform.position, transform.rotation);
            var render = obj.GetComponent<Renderer>();

            if (render) render.material = _material;
        }

        #endregion

        #region METHODS

        public void SetMaterial(Material material)
        {
            _material = material;
            foreach (var renderer in renderers)
            {
                renderer.material = material;
            }
        }

        #endregion
    }
}