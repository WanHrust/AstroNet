using UnityEngine;

namespace AstroNet.GameElements
{
    public enum FaceType {
        Square,
        Triangle,
        Circle
    }
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(Collider))]
    public class ObjectFace : MonoBehaviour
    {
        [SerializeField] private FaceType _type;
        [SerializeField] private GameObject _explosionEffect;
        [SerializeField] private float _effectDuration;

        private bool _active = true;

        public bool Active => _active;

        public FaceType Type => _type;

        private MeshRenderer _meshRenderer;
        private Collider _collider;

        protected void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _collider = GetComponent<Collider>();
        }

        public void Explode()
        {
            Instantiate(_explosionEffect, this.transform);
            Invoke("DisableObject", _effectDuration);
        }

        private void DisableObject()
        {
            _active = false;
            _meshRenderer.enabled = false;
            _collider.enabled = false;
        }
    }
}