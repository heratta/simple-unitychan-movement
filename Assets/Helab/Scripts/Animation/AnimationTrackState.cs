using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Helab.Animation
{
    public class AnimationTrackState
    {
        public bool IsValid => ClipPlayable.IsValid() || ControllerPlayable.IsValid();
        
        public AnimationClipPlayable ClipPlayable;

        public AnimatorControllerPlayable ControllerPlayable;

        public RuntimeAnimatorController Controller;

        public void SetState(AnimationTrackState otherState)
        {
            ClipPlayable = otherState.ClipPlayable;
            ControllerPlayable = otherState.ControllerPlayable;
            Controller = otherState.Controller;
        }

        public void ResetState()
        {
            ClipPlayable = default;
            ControllerPlayable = default;
            Controller = null;
        }
    }
}
