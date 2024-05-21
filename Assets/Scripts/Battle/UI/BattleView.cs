using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Pokemon
{
    public abstract class BattleView : View
    {
        public PokemonView Player;
        public PokemonView Opponent;

        public DialogueView DialogueBox;
        
        public BattleMenuView BattleMenu;
        public MovesMenuView MovesMenu;
        public PartyView PartyMenu;

        protected EventBinding<BattleMenuEvent> _battleMenuEventBinding;
        protected EventBinding<MovesMenuEvent> _movesMenuEventBinding;
        protected EventBinding<PokemonChangeEvent> _pokemonChangeEventBinding;

        protected override async Task OnEnable()
        {
            RegisterEvent(_battleMenuEventBinding, OnActionSelected);
            RegisterEvent(_movesMenuEventBinding, OnMoveSelected);
            RegisterEvent(_pokemonChangeEventBinding, OnPokemonChange);

            await base.OnEnable();

            Player.gameObject.SetActive(true);
            Opponent.gameObject.SetActive(true);
            BattleMenu.gameObject.SetActive(true);
            PartyMenu.gameObject.SetActive(true);
        }
        protected override async Task OnDisable()
        {
            UnRegisterEvent(_battleMenuEventBinding);
            UnRegisterEvent(_movesMenuEventBinding);
            UnRegisterEvent(_pokemonChangeEventBinding);

            await base.OnDisable();
        }
    
        private void OnActionSelected(BattleMenuEvent @event)
        {
            switch (@event.Action)
            {
                case BattleAction.Battle:
                    EnableView(MovesMenu);
                    break;
                
                default: return;
            }
        }
        private void OnMoveSelected(MovesMenuEvent @event)
        {
            int index = @event.Index;
            if (index == 4) EnableView(BattleMenu);
        }
        private void OnPokemonChange(PokemonChangeEvent @event)
        {
            Debug.Log(@event.Index + 1);
        }

        public void Set(PokemonBattleModel model)
        {
            Player.Set(model.GetPlayerPokemon());
            Opponent.Set(model.GetOpponentPokemon());
            MovesMenu.Set(model.PlayerMoves, model.GetOpponentPokemon()._types);
            PartyMenu.Set(model.Player.ToList());
        }

        private void EnableView(View view) => view.gameObject.SetActive(true);
        private void RegisterEvent<T>(EventBinding<T> eventBinding, Action<T> callback) where T : IEvent
        {
            eventBinding = new EventBinding<T>(callback);
            EventBus<T>.Register(eventBinding);
        }
        private void UnRegisterEvent<T>(EventBinding<T> eventBinding) where T : IEvent
        {
            EventBus<T>.UnRegister(eventBinding);
        }
    }
}
