using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pokemon
{
    public class TrainerController : PlayerController
    {
        [SerializeField] private List<PlayerPokemon> _pokemon;
        [SerializeField] private PokemonData _pokemonData;
        [SerializeField] public Transform _pokemonPivot;

        public PlayerPokemon ActivePokemon { get; private set; }
        public List<WildPokemon> _opponentTeam;
        public bool _inBattle = false;

        private EventBinding<BattleStartEvent> _battleEventBinding;
        private EventBinding<BattleOverEvent> _battleOverEventBinding;

        protected override void OnEnable()
        {
            base.OnEnable();

            _battleEventBinding = new EventBinding<BattleStartEvent>(EnterBattle);
            EventBus<BattleStartEvent>.Register(_battleEventBinding);
            
            _battleOverEventBinding = new EventBinding<BattleOverEvent>(ExitBattle);
            EventBus<BattleOverEvent>.Register(_battleOverEventBinding);
        }
        protected override void Awake()
        {
            base.Awake();

            var battleState = new BattleState(this, _animator);
            
            Any(battleState, new FuncPredicate(() => _inBattle));
            At(battleState, new FuncPredicate(() => !_inBattle), _stateMachine._defaultState);
        }
        protected override void Start()
        {
            base.Start();
            ActivePokemon = GetActivePokemon();
            ActivePokemon.SetBattleData(_pokemonData);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventBus<BattleStartEvent>.UnRegister(_battleEventBinding);
            EventBus<BattleOverEvent>.UnRegister(_battleOverEventBinding);
        }

        protected override void UpdateAnimator()
        {
            base.UpdateAnimator();
        }

        public PlayerPokemon GetActivePokemon()
        {
            foreach (var pokemon in _pokemon)
            {
                if (pokemon._isAlive) return pokemon;
            }

            return null;
        }
        public List<PlayerPokemon> GetPokemon()
        {
            return _pokemon;
        }

        private void EnterBattle(BattleStartEvent battleEvent)
        {
            _input.DisablePlayerActions();
            
            _inBattle = true;
            _opponentTeam = battleEvent._opponentTeam;

            _rigidbody.velocity = Vector3.zero;

            ActivePokemon.gameObject.SetActive(true);
        }
        private void ExitBattle()
        {
            _input.EnablePlayerActions();
            _inBattle = false;
            ActivePokemon.gameObject.SetActive(false);
        }
    }
}