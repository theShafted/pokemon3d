using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Pokemon
{
    public class BattleStartState : BattleBaseState
    {
        public BattleStartState(BattleController controller, BattleUI battleUI) : base(controller, battleUI) {}

        public override async void OnEnter()
        {
            _pokemon = _controller._playerTeam.First();
            _opponent = _controller._opponentTeam.First();

            await SetupUI();
            _controller._loadedUI = true;
        }
        public override void OnExit() {}

        private async Task SetupUI()
        { 
            _battleUI.gameObject.SetActive(true);

            var message = _controller._battleType == BattleType.WildPokemon ? MessageType.WildBattle : MessageType.TrainerBattle;
            
            await _battleUI.TypeDialogue(GetMessage(_opponent._name, suffix: message));
            await _battleUI.TypeDialogue(GetMessage(_pokemon._name, MessageType.PokemonSent));
            await SetupPokemon();
            
            SetupMoves();
        }

        private async Task SetupPokemon()
        {
            _battleUI.UpdateActivePokemon(_battleUI._playerPokemon, _pokemon);
            _battleUI.UpdateActivePokemon(_battleUI._opponentPokemon, _opponent);
            
            await Task.WhenAll
            (
                _battleUI.EnableMenu(_battleUI._playerPokemon._canvasGroup),
                _battleUI.EnableMenu(_battleUI._opponentPokemon._canvasGroup)
            );
        }
        private void SetupMoves()
        {
            _battleUI.ClearMoves();

            for (int i = 0; i < _pokemon._moves.Count; i++)
            {
                Move move = _pokemon._moves[i];
                MoveUI moveUI = _battleUI._moves[i];
                
                _battleUI.UpdateMove(move, moveUI, _pokemon, _opponent);
            } 
        }
    }
}