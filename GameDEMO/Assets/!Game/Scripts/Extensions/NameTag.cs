namespace _Game.Scripts.Extensions
{
    using UnityEngine;

    public class NameTag : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new(0, 2f, 0);
        [SerializeField] private bool faceCamera = true;

        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            if (!target) return;

            var worldOffset =
                target.right * offset.x +
                target.up * offset.y +
                target.forward * offset.z;

            transform.position = Vector3.Lerp(transform.position, target.position + worldOffset, Time.deltaTime * 5f);

            transform.forward = _mainCamera.transform.forward;
        }
    }
}