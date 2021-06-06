using UnityEngine;

namespace Helab.Simply
{
    public class SetAppTargetFrameRate : MonoBehaviour
    {
        [SerializeField] private int targetFrameRate = 60;
        
        private void Start()
        {
            Application.targetFrameRate = targetFrameRate;
        }
    }
}
