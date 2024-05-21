using System.Threading.Tasks;
using UnityEngine;

namespace Pokemon
{
    public class BattleOverState : BattleBaseState
    {
        public BattleOverState(BattleController controller, BattleUI battleUI) : base(controller, battleUI)
        {
        }
        public override async void OnEnter()
        {
            Debug.Log("Battle over state entered");

            _controller._loadedUI = false;
            _controller._battleOver = false;

            await DisableUI(false);

            EventBus<BattleOverEvent>.Raise(new BattleOverEvent());
        }
        public override void OnExit()
        {
            // Debug.Log("Battle over state exited");
        }

        public async Task DisableUI(bool enable)
        {
            await Task.WhenAll
            (
                _battleUI.EnableMenu(_battleUI._playerPokemon._canvasGroup, enable),
                _battleUI.EnableMenu(_battleUI._opponentPokemon._canvasGroup, enable)
            );

            _battleUI.gameObject.SetActive(enable);
        }
    }
}