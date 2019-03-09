using System.Collections.Generic;
using UnityEngine;

namespace AstroNet.GameElements
{
    [RequireComponent(typeof(Rigidbody))]
    public class AssembledObject : MonoBehaviour
    {
        [SerializeField] Vector2 _initialTorqueRange;
        [SerializeField] Vector2 _initialForceRange;
        [SerializeField] Transform _target;
        [SerializeField] List<Transform> _faces;

        private Rigidbody _rigidBody;
        // Start is called before the first frame update
        public void Attack(FaceType type)
        {
            foreach (var face in _faces)
            {
                var objectFace = face.GetComponent<ObjectFace>();
                if (!objectFace.Active) continue;
                if (objectFace.Type == type)
                {
                    objectFace.Explode();
                    break;
                }

            }
        }

        protected void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            ApplyInitialTorque();
            ApplyInitialForce();

        }

        [ContextMenu("ApplyTorque")]
        private void ApplyInitialTorque()
        {
            _rigidBody.angularVelocity = Vector3.zero;
            var tx = Random.Range(_initialTorqueRange.x, _initialTorqueRange.y);
            var ty = Random.Range(_initialTorqueRange.x, _initialTorqueRange.y);
            var tz = Random.Range(_initialTorqueRange.x, _initialTorqueRange.y);
            var randomTorque = new Vector3(tx, ty, tz) * _rigidBody.mass;

            _rigidBody.AddRelativeTorque(randomTorque, ForceMode.VelocityChange);
        }
        [ContextMenu("ApplyForce")]
        private void ApplyInitialForce()
        {
            _rigidBody.velocity = Vector3.zero;
            var direction = _target.position - transform.position;
            var force = direction * Random.Range(_initialForceRange.x, _initialForceRange.y);
            _rigidBody.AddForce(force, ForceMode.VelocityChange);
        }
    }
}