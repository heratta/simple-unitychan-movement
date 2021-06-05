using System.Linq;
using UnityEngine;

namespace Helab.Movement
{
    [DefaultExecutionOrder(10)]
    [RequireComponent(typeof(CharacterController))]
    public class Movement : MonoBehaviour
    {
        private CharacterController _characterController;

        private KineticEnergy[] _kineticEnergies;
        
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _kineticEnergies = GetComponents<KineticEnergy>();
        }
        
        private void Update()
        {
            var deltaMovement = _kineticEnergies.Aggregate(
                Vector3.zero,
                (current, ke) => current + ke.DeltaMovement);
            _characterController.Move(deltaMovement);
        }
    }
}
