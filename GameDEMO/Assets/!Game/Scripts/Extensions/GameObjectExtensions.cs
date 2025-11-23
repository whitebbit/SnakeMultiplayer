using UnityEngine;

namespace _Game.Scripts.Extensions
{
    public static class GameObjectExtensions
    {
        public static void SetLayer(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            SetLayerRecursive(gameObject, layer);
        }

        public static void SetLayer(this GameObject gameObject, string layerName)
        {
            var layer = LayerMask.NameToLayer(layerName);

            SetLayerRecursive(gameObject, layer);
        }

        public static void SetLayer(this GameObject gameObject, LayerMask layer)
        {
            var layerIndex = Mathf.RoundToInt(Mathf.Log(layer.value, 2));

            SetLayerRecursive(gameObject, layerIndex);
        }

        private static void SetLayerRecursive(GameObject obj, int layerIndex)
        {
            obj.layer = layerIndex;

            foreach (Transform child in obj.transform)
            {
                SetLayerRecursive(child.gameObject, layerIndex);
            }
        }
    }
}