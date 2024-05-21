using UnityEngine;

namespace Pokemon
{
    public class JumpState : BaseState
    {
        public JumpState(PlayerController controller, Animator animator) : base(controller, animator) {}

        public override void OnEnter()
        {
            _animator.CrossFade(_jumpHash, _fadeDuration);
        }
        public override void Update()
        {
        
        }
        public override void FixedUpdate()
        {
            _controller.UpdateJump();
            // _controller.UpdateMovement();
        }
    }
}