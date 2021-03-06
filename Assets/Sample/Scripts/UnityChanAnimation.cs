using System;
using Helab.Animation;
using Helab.Movement;
using UnityEngine;

namespace Sample
{
    [RequireComponent(typeof(ThrustEnergy))]
    [RequireComponent(typeof(JumpEnergy))]
    public class UnityChanAnimation : MonoBehaviour
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        
        private ThrustEnergy _thrustEnergy;
        
        private JumpEnergy _jumpEnergy;
        
        private PlayableAnimator _playableAnimator;
        
        private enum AnimationState
        {
            Idle,
            Run,
            Jump,
        }

        private AnimationState _animationState;
        
        private string _currentAssetName;

        private void Start()
        {
            _thrustEnergy = GetComponent<ThrustEnergy>();
            _jumpEnergy = GetComponent<JumpEnergy>();
            _playableAnimator = GetComponentInChildren<PlayableAnimator>();
            _animationState = AnimationState.Idle;
            
            SetAnimation(GetAssetNameFromState(), false);
        }

        private void Update()
        {
            UpdateAnimationState();
            SetAnimation(GetAssetNameFromState(), true);
        }

        private void SetAnimation(string assetName, bool isBlend)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                return;
            }
            
            if (_currentAssetName == assetName)
            {
                return;
            }

            if (_playableAnimator.SetAnimation(assetName, isBlend))
            {
                _currentAssetName = assetName;
            }
        }
        
        private string GetAssetNameFromState()
        {
            return _animationState switch
            {
                AnimationState.Idle => "WAIT00",
                AnimationState.Run  => "UnityChanLocomotions",
                AnimationState.Jump => "JUMP00",
                _                   => throw new ArgumentOutOfRangeException()
            };
        }
        
        private void UpdateAnimationState()
        {
            switch (_animationState)
            {
            case AnimationState.Idle:
                {
                    if (_jumpEnergy.IsEnabledUpdate)
                    {
                        _animationState = AnimationState.Jump;
                    }
                    else if (_thrustEnergy.IsEnabledUpdate)
                    {
                        _animationState = AnimationState.Run;
                    }
                }
                break;
            case AnimationState.Run:
                {
                    if (_jumpEnergy.IsEnabledUpdate)
                    {
                        _animationState = AnimationState.Jump;
                    }
                    else if (!_thrustEnergy.IsEnabledUpdate)
                    {
                        _animationState = AnimationState.Idle;
                    }
                    
                    _playableAnimator.SetValueToAnimator(Speed, _thrustEnergy.ThrustMeasure);
                }
                break;
            case AnimationState.Jump:
                {
                    if (!_jumpEnergy.IsEnabledUpdate)
                    {
                        _animationState = _thrustEnergy.IsEnabledUpdate ?
                            AnimationState.Run : AnimationState.Idle;
                    }
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}
