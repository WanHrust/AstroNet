using System;
using System.Collections.Generic;
using UnityEngine;

namespace AstroNet.GameElements
{
    [RequireComponent(typeof(Rigidbody))]
    public class AssembledObject : MonoBehaviour
    {
        public Action OnAsteroidDestroyed;

        [SerializeField] Transform _target;
        [SerializeField] List<Transform> _faces;

        private Rigidbody _rigidBody;
        // Start is called before the first frame update
        public void Attack(FaceType type)
        {
            int numOfInactive = 0;
            foreach (var face in _faces)
            {
                var objectFace = face.GetComponent<ObjectFace>();
                if (!objectFace.Active)
                {
                    numOfInactive++;
                    continue;
                }
                if (objectFace.Type == type)
                {
                    objectFace.Explode();
                    break;
                }
            }
            if(numOfInactive == _faces.Count - 1)
            {
                OnAsteroidDestroyed?.Invoke();
            }
        }

        protected void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        public void ApplyInitialTorque(Vector3 torque)
        {
            _rigidBody.angularVelocity = Vector3.zero;
            var randomTorque = torque * _rigidBody.mass;
            _rigidBody.AddRelativeTorque(randomTorque, ForceMode.VelocityChange);
        }

        public void ApplyInitialForce(Vector3 force)
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.AddForce(force, ForceMode.VelocityChange);
        }
    }
}