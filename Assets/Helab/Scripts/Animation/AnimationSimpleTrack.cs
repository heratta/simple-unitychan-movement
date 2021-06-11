using UnityEngine;

namespace Helab.Animation
{
    public static class AnimationSimpleTrack
    {
        public static bool SetAnimation(PlayableAnimator playableAnimator, object asset)
        {
            if (asset == null)
            {
                return false;
            }

            var ret = true;
            switch (asset)
            {
            case AnimationClip clip:
                {
                    var clipPlayable = playableAnimator.CreateClipPlayable(clip);
                    playableAnimator.SetSourcePlayable(clipPlayable);
                    playableAnimator.SetCurrentPlayable(clipPlayable);
                    break;
                }
            case RuntimeAnimatorController controller:
                {
                    var controllerPlayable = playableAnimator.CreateControllerPlayable(controller);
                    playableAnimator.SetSourcePlayable(controllerPlayable);
                    playableAnimator.SetCurrentPlayable(controllerPlayable, controller);
                    break;
                }
            default:
                {
                    ret = false;
                    break;
                }
            }

            return ret;
        }
    }
}
