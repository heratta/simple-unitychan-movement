using Helab.Movement;
using Helab.Simply;
using UnityEngine;

namespace Sample
{
    [DefaultExecutionOrder(-10)]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Movement))]
    [RequireComponent(typeof(GravityEnergy))]
    [RequireComponent(typeof(ThrustEnergy))]
    [RequireComponent(typeof(JumpEnergy))]
    [RequireComponent(typeof(TurnAroundWithDirection))]
    public class UnityChanController : MonoBehaviour
    {
        [SerializeField] private SimpleCamera simpleCamera;

        private CharacterController _characterController;
        
        private GravityEnergy _gravityEnergy;
        
        private ThrustEnergy _thrustEnergy;
        
        private JumpEnergy _jumpEnergy;
        
        private TurnAroundWithDirection _turnAround;

        private enum CharacterJumpState
        {
            Idle,
            Ready,
            Jumping,
            Grounded,
        }
        
        private CharacterJumpState _jumpState;

        private float _jumpWaitTime;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _gravityEnergy = GetComponent<GravityEnergy>();
            _thrustEnergy = GetComponent<ThrustEnergy>();
            _jumpEnergy = GetComponent<JumpEnergy>();
            _turnAround = GetComponent<TurnAroundWithDirection>();

            _jumpEnergy.IsEnabledUpdate = false;
        }

        public void Update()
        {
            var isGrounded = CheckOnGround();
            UpdateForGravityEnergy(isGrounded);
            
            var (thrustDirection, thrustMeasure) = GetThrustDirectionAndMeasure();
            UpdateJump(Time.deltaTime, isGrounded, thrustDirection, thrustMeasure, out var isAllowThrustMovement, out var adjustmentRateOfThrustMeasure);
            
            var isEnabledThrustUpdate = CheckIfThrustMovable(isAllowThrustMovement, thrustDirection);
            UpdateForThrustEnergy(isEnabledThrustUpdate, thrustDirection, thrustMeasure, adjustmentRateOfThrustMeasure);

            if (isEnabledThrustUpdate)
            {
                UpdateForTurnAround(thrustDirection);
            }
        }

        private void UpdateForGravityEnergy(bool isGrounded)
        {
            _gravityEnergy.IsGrounded = isGrounded;
        }

        private void UpdateForThrustEnergy(bool isEnabled, Vector3 thrustDirection, float thrustMeasure, float adjustmentRateOfThrustMeasure)
        {
            _thrustEnergy.IsEnabledUpdate = isEnabled;
            _thrustEnergy.ThrustDirection = thrustDirection;
            _thrustEnergy.ThrustMeasure = thrustMeasure * adjustmentRateOfThrustMeasure;
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

        private static bool CheckIfThrustMovable(bool isAllowThrustMovement, Vector3 thrustDirection)
        {
            return isAllowThrustMovement && 0f < thrustDirection.magnitude;
        }
        
        private void UpdateJump(float deltaTime, bool isGrounded, Vector3 thrustDirection, float thrustMeasure, out bool isAllowThrustMovement, out float adjustmentRateOfThrustMeasure)
        {
            switch (_jumpState)
            {
            case CharacterJumpState.Idle:
                {
                    var doJump = Input.GetButtonDown("Jump");
                    if (doJump)
                    {
                        _jumpState = CharacterJumpState.Ready;
                        _jumpWaitTime = 0.25f;
                        _jumpEnergy.IsEnabledUpdate = true;
                        _jumpEnergy.JumpDirection = Vector3.zero;
                    }
                }
                break;
            case CharacterJumpState.Ready:
                {
                    _jumpWaitTime -= deltaTime;
                    if (_jumpWaitTime <= 0f)
                    {
                        _jumpState = CharacterJumpState.Jumping;
                        _jumpEnergy.JumpDirection = Vector3.up + thrustDirection * thrustMeasure;
                        _jumpEnergy.JumpDirection.Normalize();
                    }
                }
                break;
            case CharacterJumpState.Jumping:
                {
                    if (isGrounded)
                    {
                        _jumpState = CharacterJumpState.Grounded;
                        _jumpWaitTime = 0.4f;
                        _jumpEnergy.JumpDirection = Vector3.zero;
                    }
                }
                break;
            case CharacterJumpState.Grounded:
                {
                    _jumpWaitTime -= deltaTime;
                    if (_jumpWaitTime <= 0f)
                    {
                        _jumpState = CharacterJumpState.Idle;
                        _jumpEnergy.IsEnabledUpdate = false;
                    }
                }
                break;
            }
            
            switch (_jumpState)
            {
            case CharacterJumpState.Jumping:
            case CharacterJumpState.Grounded:
                {
                    isAllowThrustMovement = false;
                }
                break;
            default:
                {
                    isAllowThrustMovement = true;
                }
                break;
            }

            adjustmentRateOfThrustMeasure = _jumpState == CharacterJumpState.Ready ? 0.5f : 1.0f;
        }
    }
}
