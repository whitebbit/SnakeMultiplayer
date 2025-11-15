using UnityEngine;

namespace _Game.Scripts.Cameras
{
    public class CameraManager : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        #endregion

        #region FIELDS

        #endregion

        #region UNITY FUNCTIONS

        private void Start()
        {
            if (!Camera.main) return;
            
            var cam = Camera.main.transform;
            
            cam.SetParent(transform);
            cam.localPosition = Vector3.zero;
        }

        private void OnDestroy()
        {
            if (!Camera.main) return;
            
            var cam = Camera.main.transform;
            cam.SetParent(null);
        }

        #endregion

        #region METHODS

        #endregion
    }
}