using UnityEngine;

namespace Helab.Simply
{
    public class TurnAroundWithDirection : MonoBehaviour
    {
        [SerializeField] private float turnSpeed = 1.0f;
        
        public Vector3 TargetDirection { get; set; } = Vector3.forward;
        
        private Vector3 _viewDirection = Vector3.forward;

        private void Start()
        {
            SetRotation(_viewDirection);
        }

        private void LateUpdate()
        {
            UpdateDirection(Time.deltaTime, TargetDirection);
        }

        private void SetRotation(Vector3 direction)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
        
        private void UpdateDirection(float deltaTime, Vector3 direction)
        {
            if (Vector3.Distance(_viewDirection, direction) <= 0.001f)
            {
                return;
            }
            
            _viewDirection = Vector3.Lerp(_viewDirection, direction, turnSpeed * deltaTime);
            SetRotation(_viewDirection);
        }
    }
}
