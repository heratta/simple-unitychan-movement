using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Helab.Animation
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AnimationAssets))]
    public class PlayableAnimator : MonoBehaviour
    {
        public bool IsPause { get; set; }
        
        private Animator _animator;
        
        private AnimationAssets _assets;
        
        private AnimationBlendableTrack _blendableTrack;

        private AnimationTrackState _prevState;

        private AnimationTrackState _currentState;

        private bool _isBlend;

        private object _nextAsset;
        
        private PlayableGraph _playableGraph;
        
        private AnimationPlayableOutput _playableOutput;

        public bool SetAnimation(string assetName, bool isBlend)
        {
            if (!_playableGraph.IsValid())
            {
                Debug.LogWarning("Playable graph is invalid.");
                return false;
            }

            var asset = _assets.FindAsset(assetName);
            if (asset == null)
            {
                Debug.LogWarning($"Not found asset: {assetName}");
                return false;
            }

            _nextAsset = asset;
            _isBlend = isBlend;
            
            return true;
        }
        
        public void StopAnimation()
        {
            _blendableTrack.StopAnimation(this);
            _playableGraph.Stop();
        }
        
        public AnimationMixerPlayable CreateMixerPlayable(int inputCount)
        {
            return AnimationMixerPlayable.Create(_playableGraph, inputCount, true);
        }
        
        public AnimationClipPlayable CreateClipPlayable(AnimationClip clip)
        {
            return AnimationClipPlayable.Create(_playableGraph, clip);
        }

        public AnimatorControllerPlayable CreateControllerPlayable(RuntimeAnimatorController controller)
        {
            return AnimatorControllerPlayable.Create(_playableGraph, controller);
        }
        
        public void Connect<TSource>(AnimationMixerPlayable mixerPlayable, TSource anyPlayable, int inputIndex)
            where TSource : struct, IPlayable
        {
            _playableGraph.Connect(anyPlayable, _playableOutput.GetSourceOutputPort(), mixerPlayable, inputIndex);
        }
        
        public void Disconnect(AnimationMixerPlayable mixerPlayable, int inputIndex)
        {
            _playableGraph.Disconnect(mixerPlayable, inputIndex);
        }
        
        public void SetSourcePlayable<TSource>(TSource anyPlayable)
            where TSource : struct, IPlayable
        {
            _playableOutput.SetSourcePlayable(anyPlayable);
        }
        
        public void SetCurrentPlayable(AnimationClipPlayable clipPlayable)
        {
            _currentState.ClipPlayable = clipPlayable;
        }
        
        public void SetCurrentPlayable(AnimatorControllerPlayable controllerPlayable, RuntimeAnimatorController controller)
        {
            _currentState.ControllerPlayable = controllerPlayable;
            _currentState.Controller = controller;
        }

        public void SetValueToAnimator(int hash, float value)
        {
            _animator.SetFloat(hash, value);
        }

        private void Awake()
        {
            CreateGraph(this.name, GetComponent<Animator>());
            _assets = GetComponent<AnimationAssets>();
            _blendableTrack = new AnimationBlendableTrack(this);
            _prevState = new AnimationTrackState();
            _currentState = new AnimationTrackState();
        }

        private void OnDisable()
        {
            if (_playableGraph.IsValid())
            {
                _playableGraph.Destroy();
            }
        }

        private void Update()
        {
            EarlyUpdateAnimation();
            UpdateAnimation();
        }
        
        private void CreateGraph(string graphName, Animator animator)
        {
            var playableGraph = PlayableGraph.Create(graphName);
            playableGraph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
            var playableOutput = AnimationPlayableOutput.Create(playableGraph, graphName, animator);

            _playableGraph = playableGraph;
            _playableOutput = playableOutput;
            _animator = animator;
        }

        private void EarlyUpdateAnimation()
        {
            if (_nextAsset == null)
            {
                return;
            }
            
            _prevState.SetState(_currentState);
            _currentState.ResetState();

            if (_isBlend)
            {
                _blendableTrack.SetAnimation(this, _nextAsset, _prevState);
            }
            else
            {
                AnimationSimpleTrack.SetAnimation(this, _nextAsset);
            }
                
            _nextAsset = null;
        }

        private void UpdateAnimation()
        {
            if (IsPause || !_currentState.IsValid)
            {
                return;
            }

            var deltaTime = Time.deltaTime;
            if (_isBlend)
            {
                _blendableTrack.UpdateBlend(deltaTime);
            }

            _playableGraph.Evaluate(deltaTime);
        }
    }
}
