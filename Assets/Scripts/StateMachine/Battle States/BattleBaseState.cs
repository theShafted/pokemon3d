using UnityEngine;

namespace Pokemon
{
    public class BattleBaseState : IState
    {
        protected BattleController _controller;
        protected BattleUI _battleUI;
        protected PlayerPokemon _pokemon;
        protected WildPokemon _opponent;

        public BattleBaseState(BattleController controller, BattleUI battleUI)
        {
            _controller = controller;
            _battleUI = battleUI;
        }

        public virtual void FixedUpdate() {}
        public virtual void OnEnter() {}
        public virtual void OnExit() {}
        public virtual void Update() {}
        protected virtual string GetMessage(string name, MessageType prefix = MessageType.None, MessageType suffix = MessageType.None)
        {
            return _controller.GetMessage(prefix) + name + _controller.GetMessage(suffix);
        }
    }
}