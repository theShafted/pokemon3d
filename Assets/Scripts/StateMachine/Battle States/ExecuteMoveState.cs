using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Pokemon
{
    public class ExecuteMoveState : BattleBaseState
    {
        Pokemon _attacker;
        Pokemon _defender;
        Move _move;

        public ExecuteMoveState(BattleController controller, BattleUI battleUI) : base(controller, battleUI) {}

        public override async void OnEnter()
        {
            _pokemon = _controller._playerTeam.First();
            _opponent = _controller._opponentTeam.First();
            
            if (PlayerTurn()) await ExecuteTurn(_pokemon, _opponent);
            else await ExecuteTurn(_opponent, _pokemon);

            if (_controller._battleOver) return;
            
            await ExecuteTurn(_defender, _attacker);

            _controller._executeMove = null;
        }

        private bool PlayerTurn()
        {
            int pokemonSpeed = _pokemon.GetStatValue(StatType.Speed);
            int opponentSpeed = _opponent.GetStatValue(StatType.Speed);

            if (pokemonSpeed == opponentSpeed) return Random.Range(0, 1) > 0.5; 
            
            return pokemonSpeed > opponentSpeed;
        }
        private async Task ExecuteTurn(Pokemon attacker, Pokemon defender)
        {
            _attacker = attacker;
            _defender = defender;
            _move = _attacker == _pokemon ? _controller._executeMove : _opponent.GetMove();

            await UpdateMessage();
            await ExecuteMove();
        }
        private async Task UpdateMessage()
        {
            var message = GetMessage(_attacker._name, suffix: MessageType.MoveUsed);
            await _battleUI.TypeDialogue(message + _move._moveData._name);
        }
        private async Task ExecuteMove()
        {
            if (_move._moveData._category != MoveType.Status) await ExecuteDamage();
            await ModifyStats(_move._moveData._effect);
        }
        private async Task ExecuteDamage()
        {
            DamageData damageData = _defender.TakeDamage(_move, _attacker);
            ActivePokemonUI defenderUI = _attacker == _pokemon ? _battleUI._opponentPokemon : _battleUI._playerPokemon;

            _battleUI.UpdateHP(_defender, defenderUI);

            await TypeMessage(damageData._type);

            if (damageData._critical)
                await _battleUI.TypeDialogue(GetMessage("", MessageType.Critical));

            _attacker._moveUsed = null;
            _defender._damaged = false;
            
            if (damageData._fainted)
            {
                await _battleUI.TypeDialogue(GetMessage(_defender._name, suffix: MessageType.Faint));
                _controller._battleOver = true;
                _controller._executeMove = null;
            }
        }
        private async Task<bool> ModifyStats(MoveEffect effect)
        {
            if (effect._stats.Count <= 0) return false;
         
            foreach (Stat stat in effect._stats)
            {
                Pokemon target = effect._target == MoveTarget.Self ? _attacker : _defender;
                target.UpdateModifier(stat);
                
                await StatMessage(target, stat);

                var ui = target == _pokemon ? _battleUI._playerPokemon : _battleUI._opponentPokemon;
                ui.UpdateStat(stat._stat, target._statStages[stat._stat]);
            }

            return true;
        }
        private async Task TypeMessage(float type)
        {
            var messageType = type switch
            {
                0.0f => MessageType.InEffective,
                0.5f => MessageType.NotEffective,
                2.0f => MessageType.SuperEffective,
                _ => MessageType.None,
            };

            if (messageType != MessageType.None)
                await _battleUI.TypeDialogue(GetMessage(_opponent._name, messageType));
        }
        private async Task StatMessage(Pokemon target, Stat stat)
        {
            string targetStat = target._name + "'s " + stat._stat;
            MessageType modifier = stat._value > 0 ? MessageType.StatIncreased : MessageType.StatDecreased;

            await _battleUI.TypeDialogue(GetMessage(targetStat, suffix: modifier));
        }
        
    }
}