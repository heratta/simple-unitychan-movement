using UnityEngine;

namespace Helab.Movement
{
    public class ThrustEnergy : KineticEnergy
    {
        [SerializeField] private float thrustSpeed = 1.0f;
        
        public Vector3 ThrustDirection { get; set; }
        
        public float ThrustMeasure { get; set; }
        
        protected override void UpdateKineticEnergy(float deltaTime)
        {
            if (!Physics.Raycast(transform.position, Vector3.down, out var hit))
            {
                return;
            }
            
            var right = Vector3.Cross(hit.normal, ThrustDirection);
            var forward = Vector3.Cross(right, hit.normal);
            DeltaMovement = forward.normalized * (thrustSpeed * ThrustMeasure * deltaTime);
        }
    }
}
