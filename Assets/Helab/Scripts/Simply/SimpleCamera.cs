using UnityEngine;

namespace Helab.Simply
{
    public class SimpleCamera : MonoBehaviour
    {
        [SerializeField] private float rotateSpeed = 1.0f;

        [SerializeField] private Transform followTarget;

        public Vector3 TransformToCameraSpace(Vector3 input)
        {
            var rotateY = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
            return rotateY * input;
        }

        private void Update()
        {
            UpdateCameraRotation();
        }

        private void LateUpdate()
        {
            UpdateCameraEyeTarget();
        }

        private void UpdateCameraEyeTarget()
        {
            if (followTarget == null)
            {
                return;
            }
            
            transform.position = followTarget.position;
        }

        private void UpdateCameraRotation()
        {
            var rotateValue = SimpleHelper.GetCameraRotateValueByInput();
            var deltaRotate = rotateValue.x * rotateSpeed * Time.deltaTime;
            var yDegree = transform.rotation.eulerAngles.y + deltaRotate;
            transform.rotation = Quaternion.Euler(0f, yDegree, 0f);
        }
    }
}
