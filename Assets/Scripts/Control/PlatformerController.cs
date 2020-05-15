using FridgeLogic.Movement;
using UnityEngine;

namespace FridgeLogic.Control
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlatformerCollider2d))]
    public class PlatformerController : MonoBehaviour
    {
        [SerializeField] private MovementOptions _options = null;

        private MovementController _movementController;
        private JumpController _jumpController;

        private PlatformerCollider2d _platformerCollider = null;
        private PlatformerCollider2d PlatformerCollider => _platformerCollider ?? (_platformerCollider = GetComponent<PlatformerCollider2d>());

        private Animator _animator = null;
        private Animator Animator => _animator ?? (_animator = GetComponent<Animator>());

        private Vector2 _velocity = Vector2.zero;

        public void Move(Vector2 movement) => _movementController.Move(movement);

        public void StartRunning() => _movementController.StartRunning();

        public void StopRunning() => _movementController.StopRunning();

        public void StartJumping() => _jumpController.StartJumping();

        public void StopJumping() => _jumpController.StopJumping();

        #region Animations
        private void UpdateAnimator(Vector2 translation)
        {
            if (_platformerCollider.CollisionInfo.below)
            {
                if (Mathf.Abs(translation.x) >= 0.001)
                {
                    Animator.SetFloat("FacingX", Mathf.Sign(translation.x));
                    Animator.SetFloat("HorizontalSpeed", 1f);
                    Animator.SetBool("IsRunning", _movementController.IsRunning);
                }
                else
                {
                    Animator.SetFloat("HorizontalSpeed", 0);
                    Animator.SetBool("IsRunning", false);
                }
            }

            Animator.SetBool("IsGrounded", _platformerCollider.CollisionInfo.below);
        }
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            _movementController = new MovementController(_options);
            _jumpController = new JumpController(_options);
        }

        private void Update()
        {
            _jumpController.ApplyMovement(PlatformerCollider.CollisionInfo, ref _velocity);
            _movementController.ApplyJump(PlatformerCollider.CollisionInfo, ref _velocity);
            var translation = MovementCalculator.CalculateTranslation(_jumpController.Gravity, ref _velocity);
            PlatformerCollider.ApplyCollisions(ref translation);
            transform.Translate(translation);
            UpdateAnimator(translation);
        }
        #endregion

    }
}