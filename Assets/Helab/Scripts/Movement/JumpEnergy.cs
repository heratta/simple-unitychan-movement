using UnityEngine;

namespace Helab.Movement
{
    public class JumpEnergy : KineticEnergy
    {
        [SerializeField] private float jumpSpeed = 1.0f;
        
        public Vector3 JumpDirection { get; set; }
        
        protected override void UpdateKineticEnergy(float deltaTime)
        {
            DeltaMovement = JumpDirection * (jumpSpeed * deltaTime);
        }
    }
}
