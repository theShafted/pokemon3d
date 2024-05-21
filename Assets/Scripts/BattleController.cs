using System;
using System.Collections.Generic;
using UnityEditor.VersionControl;

namespace Pokemon
{
    public class BattleController
    {
        public List<PlayerPokemon> _playerTeam;
        public List<WildPokemon> _opponentTeam;
        public BattleType _battleType;

        StateMachine _stateMachine;
        MessageData _messageData;
        public BattleUI _battleUI;

        public bool _loadedUI = false;
        public Move _executeMove = null;
        public bool _battleOver;

        private EventBinding<MoveUsedEvent> _moveUsedEventBinding;
        private EventBinding<BattleStartEvent> _battleEventBinding;
        private EventBinding<BattleOverEvent> _battleOverEventBinding;

        public BattleController(BattleUI battleUI, MessageData messageData)
        {
            _battleUI = battleUI;
            _messageData = messageData;

            _moveUsedEventBinding = new EventBinding<MoveUsedEvent>(ExecuteMove);
            EventBus<MoveUsedEvent>.Register(_moveUsedEventBinding);

            _battleEventBinding = new EventBinding<BattleStartEvent>(StartBattle);
            EventBus<BattleStartEvent>.Register(_battleEventBinding);

            _battleOverEventBinding = new EventBinding<BattleOverEvent>(EndBattle);
            EventBus<BattleOverEvent>.Register(_battleOverEventBinding);
        }
        public void StartBattle(BattleStartEvent battleEvent)
        {
            _stateMachine = new StateMachine();

            _playerTeam = battleEvent._playerTeam;
            _opponentTeam = battleEvent._opponentTeam;
            _battleType = battleEvent._type;

            var startState = new BattleStartState(this, _battleUI);
            var playerInputState = new PlayerInputState(this, _battleUI);
            var executeMoveState = new ExecuteMoveState(this, _battleUI);
            var battleOverState = new BattleOverState(this, _battleUI);


            At(startState, new FuncPredicate(() => _loadedUI && _executeMove == null), playerInputState);
            
            At(playerInputState, new FuncPredicate(() => _executeMove != null), executeMoveState);
            At(executeMoveState, new FuncPredicate(() => !_battleOver && _executeMove == null), playerInputState);
            
            At(executeMoveState, new FuncPredicate(() => _battleOver), battleOverState);

            _stateMachine.SetState(startState);
            _battleOver = false;
        }
        public void EndBattle()
        {

        }
        ~BattleController()
        {
            EventBus<MoveUsedEvent>.UnRegister(_moveUsedEventBinding);
        }
        public void Update() => _stateMachine?.Update();
        public void FixedUpdate() => _stateMachine?.FixedUpdate();

        private void ExecuteMove(MoveUsedEvent moveUsedEvent)
        {
            _executeMove = moveUsedEvent.move;
            moveUsedEvent.attacker._moveUsed = _executeMove;
        }
        public string GetMessage(MessageType messageType)
        {
            return _messageData.GetMessage(messageType);
        }
        private void At(IState from, IPredicate condition, IState to) => _stateMachine.AddTransition(from, condition, to);
        private void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);
    }
}