using UnityEngine;

namespace Pokemon
{
    public class BattleState : BaseState
    {
        public BattleState(PlayerController controller, Animator animator) : base(controller, animator) {}

        public override void OnEnter()
        {
            _animator.CrossFade(_battleHash, _fadeDuration);
            _controller.transform.position -= (_controller.transform.forward - Vector3.up) * 5;
        }
    }
}