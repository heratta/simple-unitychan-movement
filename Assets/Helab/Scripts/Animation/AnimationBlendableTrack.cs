using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Helab.Animation
{
    public class AnimationBlendableTrack
    {
        private readonly AnimationMixerPlayable _mixerPlayable;
        
        private float _blendElapsedTime;

        private float _blendDuration = 0.5f;

        public AnimationBlendableTrack(PlayableAnimator playableAnimator)
        {
            _mixerPlayable = playableAnimator.CreateMixerPlayable(2);
        }
        
        public bool SetAnimation(PlayableAnimator playableAnimator, object asset, AnimationTrackState prevState)
        {
            if (asset == null)
            {
                return false;
            }
            
            playableAnimator.Disconnect(_mixerPlayable, 0);
            playableAnimator.Disconnect(_mixerPlayable, 1);

            if (prevState.ClipPlayable.IsValid())
            {
                playableAnimator.Connect(_mixerPlayable, prevState.ClipPlayable, 1);
            }
            else if (prevState.ControllerPlayable.IsValid())
            {
                playableAnimator.Connect(_mixerPlayable, prevState.ControllerPlayable, 1);
            }
            
            var ret = true;
            switch (asset)
            {
            case AnimationClip clip:
                {
                    var clipPlayable = playableAnimator.CreateClipPlayable(clip);
                    playableAnimator.Connect(_mixerPlayable, clipPlayable, 0);
                    playableAnimator.SetCurrentPlayable(clipPlayable);
                    break;
                }
            case RuntimeAnimatorController controller:
                {
                    var controllerPlayable = playableAnimator.CreateControllerPlayable(controller);
                    playableAnimator.Connect(_mixerPlayable, controllerPlayable, 0);
                    playableAnimator.SetCurrentPlayable(controllerPlayable, controller);
                    break;
                }
            default:
                {
                    ret = false;
                    break;
                }
            }
            
            playableAnimator.SetSourcePlayable(_mixerPlayable);
            _blendElapsedTime = 0f;

            return ret;
        }

        public void StopAnimation(PlayableAnimator playableAnimator)
        {
            playableAnimator.Disconnect(_mixerPlayable, 0);
            playableAnimator.Disconnect(_mixerPlayable, 1);
        }

        public void UpdateBlend(float deltaTime)
        {
            _blendElapsedTime += deltaTime;
            if (_blendDuration <= _blendElapsedTime)
            {
                _mixerPlayable.SetInputWeight(0, 1f);
                _mixerPlayable.SetInputWeight(1, 0f);
            }
            else
            {
                var rate = Mathf.Clamp01(_blendElapsedTime / _blendDuration);
                _mixerPlayable.SetInputWeight(0, rate);
                _mixerPlayable.SetInputWeight(1, 1f - rate);
            }
        }
    }
}
