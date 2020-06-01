using System.Collections.Generic;
using UnityEngine;

namespace FridgeLogic.Damage
{
    [RequireComponent(typeof(Animator))]
    public class Trap : MonoBehaviour
    {
        [SerializeField] private float _damage = 1f;
        [SerializeField] private bool _armed = false;
        [SerializeField] private float _targetSeekRadius = 5f;
        [SerializeField] private Collider2D _trapCollider = null;
        [SerializeField] private LayerMask _targetLayer = default(LayerMask);
        
        private Transform _transform = null;
        private Animator _animator = null;

        public void Arm()
        {
            _armed = true;
        }

        public void Disarm()
        {
            _armed = false;
        }

        private void Start()
        {
            _transform = transform;
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_armed)
            {
                var colliders = new List<Collider2D>();
                _trapCollider.OverlapCollider(new ContactFilter2D()
                {
                    layerMask = _targetLayer,
                    useTriggers = true
                }, colliders);
                
                for (int i = colliders.Count - 1; i >= 0; i--)
                {
                    if (colliders[i].TryGetComponent<Health>(out var health))
                    {
                        health.TakeDamage(_damage);
                    }
                }
            }
            else
            {
                var collider = Physics2D.OverlapCircle(_transform.position, _targetSeekRadius, _targetLayer);
                if (collider)
                {
                    _animator.SetBool("TargetNear", true);
                }
                else
                {
                    _animator.SetBool("TargetNear", false);
                }
            }
        }
    }
}
