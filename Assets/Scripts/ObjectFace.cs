using System;
using System.Collections.Generic;
using UnityEngine;

namespace AstroNet.GameElements
{
    public enum FaceType
    {
        Pentagon,
        Square,
        Triangle,
        Circle
    }
    public class ObjectFace : MonoBehaviour
    {
        [SerializeField] private FaceType _type;
        [SerializeField] private GameObject _explosionEffect;
        [SerializeField] private float _effectDuration;

        public Action OnFaceExploded;

        private bool _active = true;

        public bool Active => _active;

        public FaceType Type => _type;


        private List<MeshRenderer> _meshRenderers;
        private Collider _collider;

        protected void Start()
        {
            _meshRenderers = new List<MeshRenderer>();
            var meshRenderers = GetComponentsInChildren<MeshRenderer>();
            if (meshRenderers != null)
            {
                _meshRenderers.AddRange(meshRenderers);
            }
            var meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                _meshRenderers.Add(meshRenderer);
            }
            _collider = GetComponent<Collider>();
        }

        public void Explode()
        {
            _active = false;
            Instantiate(_explosionEffect, this.transform);
            Invoke("DisableObject", _effectDuration);
        }

        private void DisableObject()
        {
            
            foreach (var meshRenderer in _meshRenderers)
            {
                meshRenderer.enabled = false;
            }

            _collider.enabled = false;
            OnFaceExploded?.Invoke();
        }
    }
}