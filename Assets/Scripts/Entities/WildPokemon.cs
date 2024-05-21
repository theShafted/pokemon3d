using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Pokemon
{
    public class WildPokemon : Pokemon
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private PlayerDetectionController _playerDetector;
        [SerializeField] private float _wanderRadius;

        protected EventBinding<BattleStartEvent> _battleEventBinding;

        protected override void OnEnable()
        {
            _battleEventBinding = new EventBinding<BattleStartEvent>(UpdateBattleEvent);
            EventBus<BattleStartEvent>.Register(_battleEventBinding);
        }
        protected override void OnDisable()
        {            
            EventBus<BattleStartEvent>.UnRegister(_battleEventBinding);
        }
        protected override void Start()
        {
            base.Start();

            var wanderState = new PokemonWanderState(this, _animator, _agent, _wanderRadius);
            var chaseState = new PokemonChaseState(this, _animator, _agent, _playerDetector._player);
            var idleState = new PokemonIdleState(this, _animator, _playerDetector._player);

            At(wanderState, new FuncPredicate(() => _playerDetector.PlayerDetectable() && !_inBattle), chaseState);
            At(chaseState, new FuncPredicate(() => !_playerDetector.PlayerDetectable() && !_inBattle), wanderState);
            Any(idleState, new FuncPredicate(() => _inBattle && _moveUsed == null && !_damaged));

            _stateMachine.SetState(wanderState);
        }
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _agent.isStopped = true;
                
                EventBus<BattleStartEvent>.Raise(new BattleStartEvent
                {
                    _type = BattleType.WildPokemon,
                    _opponentTeam = new() {this},
                    _playerTeam = _playerDetector._player.GetComponent<TrainerController>().GetPokemon(),
                });
            }
        }

        private void UpdateBattleEvent(BattleStartEvent battleEvent)
        {            
            if (battleEvent._opponentTeam.First() == this) _inBattle = true;
            else
            {
                _agent.isStopped = true;
            }
        }

        public Move GetMove() => _moves[Random.Range(0, _moves.Count)];
    }
}