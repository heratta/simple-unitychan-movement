using Helab.Movement;
using Helab.Simply;
using UnityEngine;

namespace Sample
{
    [DefaultExecutionOrder(-10)]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(GravityEnergy))]
    [RequireComponent(typeof(ThrustEnergy))]
    [RequireComponent(typeof(TurnAroundWithDirection))]
    public class UnityChanController : MonoBehaviour
    {
        [SerializeField] private SimpleCamera simpleCamera;

        private CharacterController _characterController;
        
        private GravityEnergy _gravityEnergy;
        
        private ThrustEnergy _thrustEnergy;
        
        private TurnAroundWithDirection _turnAround;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _gravityEnergy = GetComponent<GravityEnergy>();
            _thrustEnergy = GetComponent<ThrustEnergy>();
            _turnAround = GetComponent<TurnAroundWithDirection>();
        }

        public void Update()
        {
            var isGrounded = CheckOnGround();
            var (thrustDirection, thrustMeasure) = GetThrustDirectionAndMeasure();
            var isEnabledThrustUpdate = CheckIfThrustMovable(thrustDirection);
            
            UpdateForGravityEnergy(isGrounded);
            UpdateForThrustEnergy(isEnabledThrustUpdate, thrustDirection, thrustMeasure);

            if (isEnabledThrustUpdate)
            {
                UpdateForTurnAround(thrustDirection);
            }
        }

        private void UpdateForGravityEnergy(bool isGrounded)
        {
            _gravityEnergy.IsGrounded = isGrounded;
        }

        private void UpdateForThrustEnergy(bool isEnabled, Vector3 thrustDirection, float thrustMeasure)
        {
            _thrustEnergy.IsEnabledUpdate = isEnabled;
            _thrustEnergy.ThrustDirection = thrustDirection;
            _thrustEnergy.ThrustMeasure = thrustMeasure;
        }

        private void UpdateForTurnAround(Vector3 direction)
        {
            _turnAround.TargetDirection = direction;
        }
        
        private bool CheckOnGround()
        {
            var isGrounded = _characterController.isGrounded;
            if (!isGrounded)
            {
                // Note: Assume zero height is the ground.
                if (transform.position.y <= 0.01f)
                {
                    isGrounded = true;
                }
            }

            return isGrounded;
        }

        private (Vector3, float) GetThrustDirectionAndMeasure()
        {
            var (thrustDirectionInput, thrustMeasure) = SimpleHelper.GetThrustDirectionByInput();
            var thrustDirection = simpleCamera.TransformToCameraSpace(thrustDirectionInput);

            return (thrustDirection, thrustMeasure);
        }

        private bool CheckIfThrustMovable(Vector3 thrustDirection)
        {
            return 0f < thrustDirection.magnitude;
        }
    }
}
