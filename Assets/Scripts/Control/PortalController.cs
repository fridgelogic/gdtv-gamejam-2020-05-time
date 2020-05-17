using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FridgeLogic.ScriptableObjects.GameEvents;
using FridgeLogic.ScriptableObjects.Providers;
using FridgeLogic.ScriptableObjects.Values;
using UnityEngine;

namespace FridgeLogic.Control
{
    [RequireComponent(typeof(Animator))]
    public class PortalController : MonoBehaviour
    {

        [SerializeField]
        private IntValue _coinsRequiredtoOpen = null;

        [SerializeField]
        private IntValue _timeLimit = null;

        [SerializeField]
        private Transform _door = null;

        [SerializeField]
        private Vector2 _endPosition = Vector2.zero;

        [SerializeField]
        private AudioClip _portalOpenSound = null;

        [SerializeField]
        private SoundPlayerProvider _soundPlayerProvider = null;

        [SerializeField]
        private GameEvent _playerEnteredPortal = null;

        private Animator _animator = null;
        private Animator Animator => _animator ?? (_animator = GetComponent<Animator>());

        private TweenerCore<Vector3, Vector3, VectorOptions> _tween;
        private Vector2 _midwayPoint = Vector2.zero;
        private bool _reachedMidpoint;
        private int _coinsRemaining;
        private bool _isActive;
        // private int _prevValue;

        public void OnTimeLimitUpdate()
        {
            // if (_prevValue == _timeLimit.Value)
            // {
            //     return;
            // }

            // _tween.Kill();
            // CloseDoor();
        }

        public void OnGetCoin()
        {
            if (_isActive)
            {
                return;
            }
            
            if (--_coinsRemaining <= 0)
            {
                ActivatePortal();
            }
        }

        public void ActivatePortal()
        {
            _isActive = true;
            Animator.SetTrigger("Activate");
            if (_soundPlayerProvider && _portalOpenSound)
            {
                _soundPlayerProvider.SoundPlayer.PlaySound(_portalOpenSound);
            }
        }

        private void CloseDoor()
        {
            // _prevValue = _timeLimit.Value;
            // if (!_reachedMidpoint)
            // {
            //     CloseDoorFirstHalf();
            // }
            // else
            // {
            //     CloseDoorSecondHalf();
            // }
        }

        private void CloseDoorFirstHalf()
        {
            _tween = _door
                .DOLocalMove(_midwayPoint, _timeLimit.Value / 2f, false)
                .SetEase(Ease.Linear)
                .OnComplete(CloseDoorSecondHalf);
        }

        private void CloseDoorSecondHalf()
        {
            _reachedMidpoint = true;
            _tween = _door
                .DOLocalMove(_endPosition, _timeLimit.Value, false)
                .SetEase(Ease.InSine);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isActive)
            {
                _playerEnteredPortal.Raise();
            }
        }

        private void Start()
        {
            _coinsRemaining = _coinsRequiredtoOpen.Value;
            var distance = new Vector2(_door.localPosition.x, _door.localPosition.y) - _endPosition;
            _midwayPoint = _endPosition + distance * 0.3f;
            CloseDoor();
        }

        private void OnEnable()
        {
            CloseDoor();
        }

        private void OnDisable()
        {
            _tween.Kill();
        }
    }
}