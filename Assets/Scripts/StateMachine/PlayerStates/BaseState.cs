using UnityEngine;

namespace Pokemon
{
    public abstract class BaseState : IState
    {
        protected readonly PlayerController _controller;
        protected readonly Animator _animator;

        protected readonly int _moveHash = Animator.StringToHash("Move");
        protected readonly int _jumpHash = Animator.StringToHash("Jump");
        protected readonly int _battleHash = Animator.StringToHash("Battle");

        protected const float _fadeDuration = 0.1f;

        protected BaseState(PlayerController controller, Animator animator)
        {
            _controller = controller;
            _animator = animator;
        }
        public virtual void FixedUpdate() {}
        public virtual void OnEnter() {}
        public virtual void OnExit() {}
        public virtual void Update() {}
    }
}