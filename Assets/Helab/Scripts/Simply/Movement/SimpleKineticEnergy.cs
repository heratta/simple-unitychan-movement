using UnityEngine;

namespace Helab.Simply.Movement
{
    [RequireComponent(typeof(SimpleMovement))]
    public abstract class SimpleKineticEnergy : MonoBehaviour
    {
        public bool IsEnabledUpdate { get; set; } = true;
        
        public Vector3 DeltaMovement { get; protected set; }

        protected SimpleMovement Movement { get; private set; }

        private void Start()
        {
            Movement = GetComponent<SimpleMovement>();
        }

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
