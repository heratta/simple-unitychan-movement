using System.Linq;
using UnityEngine;

namespace Helab.Simply.Movement
{
    [DefaultExecutionOrder(10)]
    [RequireComponent(typeof(CharacterController))]
    public class SimpleMovement : MonoBehaviour
    {
        private CharacterController _characterController;

        private SimpleKineticEnergy[] _kineticEnergies;
        
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _kineticEnergies = GetComponents<SimpleKineticEnergy>();
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
