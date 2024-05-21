using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

namespace Pokemon
{
    public abstract class Pokemon : Entity, INotifyPropertyChanged
    {
        [SerializeField] protected Animator _animator;

        public event PropertyChangedEventHandler PropertyChanged;

        protected StateMachine _stateMachine;
        
        public bool _inBattle = false;
        public Move _moveUsed;
        public bool _damaged = false;

        public string _name;
        public int _level = 5;
        public PokemonType[] _types;
        public int HP
        { 
            get { return HP; }
            private set
            {
                HP = value;
                NotifyHPChange("HP");
            }
        }

        private void NotifyHPChange(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public Dictionary<StatType, int> _stats = new(6);
        public Dictionary<StatType, int> _statStages = new(6);

        public List<Move> _moves = new();


        protected virtual void OnEnable() {}
        protected virtual void OnDisable() {}
        protected virtual void Start()
        {
            _stateMachine = new StateMachine();

            var attackState = new PokemonAttackState(this, _animator);
            var damageState = new PokemonDamageState(this, _animator);
            var faintState = new PokemonFaintState(this, _animator);
            
            Any(attackState, new FuncPredicate(() => _inBattle && _moveUsed != null));
            Any(faintState, new FuncPredicate(() => _inBattle && HP <= 0));
            Any(damageState, new FuncPredicate(() => _inBattle && _damaged));
        }
        void Update()
        {
            _stateMachine.Update();
        }
        void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }
        protected virtual void OnTriggerEnter(Collider other) {}

        protected void At(IState from, IPredicate condition, IState to) => _stateMachine.AddTransition(from, condition, to);
        protected void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);

        public void SetBattleData(PokemonData pokemonData)
        {
            _name = pokemonData._name;
            _types = pokemonData._types;

            foreach (Stat stat in pokemonData._baseStats)
            {
                _stats[stat._stat] = CalculateStat(stat._stat, stat._value);
                _statStages[stat._stat] = 0;
            }

            HP = _stats[StatType.HP];

            foreach (LearnableMove move in pokemonData._learnableMoves)
                if (move._level <= _level) _moves.Add(new Move(move._move));
        }
        public int GetStat(StatType type) => _stats[type];
        public int GetStatValue(StatType type) => Mathf.RoundToInt(GetStat(type) * StatModifiers._values[_statStages[type]]);
        public void UpdateModifier(Stat modifier) => _statStages[modifier._stat] += modifier._value;
        
        public DamageData TakeDamage(Move move, Pokemon attacker)
        {
            _damaged = true;

            MoveData moveData = move._moveData;
            bool physical = moveData.Physical();
            PokemonType[] types = new PokemonType[] {moveData._type, _types[0], _types[1]};

            float critical = UnityEngine.Random.value * 100f < 6.25f ? 2f : 1f;
            float type1 = TypeEffectiveness.Value(types[0], types[1]);
            float type2 = TypeEffectiveness.Value(types[0], types[2]);

            int attack = attacker.GetStatValue(physical ? StatType.Attack : StatType.SpecialAttack);
            int defense = GetStatValue(physical ? StatType.Defense : StatType.SpecialDefense);
            if (critical == 2f) defense = GetStat(physical ? StatType.Defense : StatType.SpecialDefense);

            float modifiers = type1 * type2 * critical;
            int damage = (int)(GetDamage(attacker._level, moveData._power, attack, defense) * modifiers);

            HP = Mathf.Max(0, HP - damage);
            
            return new()
            {
                _damage = damage,
                _critical = critical == 2f,
                _type = type1 * type2,
                _fainted = HP == 0,
            };
        }

        private float GetDamage(int level, int power, int attack, int defense)
        {
            float a = 2f * level / 5f + 2;
            float b = power * attack / defense;

            float random = UnityEngine.Random.Range(0.85f, 1f);

            return (a * b / 50f + 2f) * random;
        }
        private int CalculateStat(StatType type, int value)
        {
            int result = Mathf.FloorToInt(2f * value * _level / 100f);

            return type == StatType.HP ? (result + _level + 10) : (result + 5);
        }
    }
}