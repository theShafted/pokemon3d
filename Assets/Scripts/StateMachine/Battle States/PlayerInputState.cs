using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Pokemon
{
    public class PlayerInputState : BattleBaseState
    {
        private bool _enable;
        
        public PlayerInputState(BattleController controller, BattleUI battleUI) : base(controller, battleUI) {}

        public override async void OnEnter()
        {
            _enable = true;

            await EnableControls(_enable);
            
            _enable = false;
        }
        public override async void OnExit() => await EnableControls(_enable);

        private async Task EnableControls(bool enable)
        {
            List<Task> tasks = new()
            {
                _battleUI.EnableMenu(_battleUI._battleMenu, enable),
                _battleUI.EnableMenu(_battleUI._partyMenu, enable),
                _battleUI.EnableMenu(_battleUI._lastUsed, enable)
            };
            if (enable != true) tasks.Add(_battleUI.EnableMenu(_battleUI._movesMenu, enable));
            
            await Task.WhenAll(tasks);            
        }
    }
}