using UnityEngine;

namespace Helab.Movement
{
    public abstract class KineticEnergy : MonoBehaviour
    {
        public bool IsEnabledUpdate { get; set; } = true;
        
        public Vector3 DeltaMovement { get; protected set; }

        private void Update()
        {
            DeltaMovement = Vector3.zero;
            if (IsEnabledUpdate)
            {
                UpdateKineticEnergy(Time.deltaTime);
            }
        }

        protected abstract void UpdateKineticEnergy(float deltaTime);
    }
}
