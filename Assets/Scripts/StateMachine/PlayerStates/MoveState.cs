using UnityEngine;

namespace Pokemon
{
    public class MoveState : BaseState
    {
        public MoveState(PlayerController controller, Animator animator) : base(controller, animator) {}

        public override void OnEnter()
        {
            _animator.CrossFade(_moveHash, _fadeDuration);
        }
        public override void FixedUpdate()
        {
            _controller.UpdateMovement();
        }
    }
}