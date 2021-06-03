using UnityEngine;

namespace Helab.Simply.Movement
{
    public class SimpleGravityEnergy : SimpleKineticEnergy
    {
        [SerializeField] private float gravitationalAcceleration = 9.8f;

        private float _gravitySpeed;

        public bool IsGrounded { get; set; }

        protected override void UpdateKineticEnergy(float deltaTime)
        {
            if (IsGrounded)
            {
                _gravitySpeed = 0f;
            }
            else
            {
                _gravitySpeed += gravitationalAcceleration * deltaTime;
                DeltaMovement = Vector3.down * (_gravitySpeed * deltaTime);
            }
        }
    }
}
