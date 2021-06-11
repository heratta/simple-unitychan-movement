using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Helab.Animation
{
    public class AnimationAssets : MonoBehaviour
    {
        [SerializeField] private List<AnimationClip> clips;
        
        [SerializeField] private List<RuntimeAnimatorController> controllers;

        private Dictionary<string, object> _assets;

        public void Awake()
        {
            _assets = new Dictionary<string, object>();
            
            foreach (var clip in clips.Where(clip => !_assets.ContainsKey(clip.name)))
            {
                _assets.Add(clip.name, clip);
            }
            
            foreach (var controller in controllers.Where(controller => !_assets.ContainsKey(controller.name)))
            {
                _assets.Add(controller.name, controller);
            }
        }
        
        public object FindAsset(string assetName)
        {
            _assets.TryGetValue(assetName, out var asset);
            return asset;
        }
    }
}
