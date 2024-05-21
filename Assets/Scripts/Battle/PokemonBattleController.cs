using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace Pokemon
{
    public class PokemonBattleController
    {
        readonly PokemonBattleView _view;
        readonly PokemonBattleModel _model;
        readonly BattleType _type;

        private EventBinding<MovesMenuEvent> _moveUsedEventBinding;

        PokemonBattleController(PokemonBattleView view, PokemonBattleModel model, BattleType type)
        {
            _view = view;
            _model = model;
            _type = type;

            Initialize();
        }

        void Initialize()
        {
            RegisterEvent(_moveUsedEventBinding, ExecuteMove);

            _view.gameObject.SetActive(true);

            _model.OnPropertyChanged += UpdateModelChange;
            
            RefreshView();
        }

        private void ExecuteMove(MovesMenuEvent @event)
        {
            var player = _model.GetPlayerPokemon();
            var opponent = _model.GetOpponentPokemon();
            var damage = opponent.TakeDamage(_model.PlayerMoves[@event.Index], player);

            Debug.Log(damage._damage);

            damage = player.TakeDamage(_model.OpponentMoves[0], opponent);
            Debug.Log(damage._damage);
        }
        private void UpdateModelChange(object sender, PropertyChangedEventArgs e) => RefreshView();
        private void RefreshView() => _view.Set(_model);
        private void RegisterEvent<T>(EventBinding<T> eventBinding, Action<T> callback) where T : IEvent
        {
            eventBinding = new EventBinding<T>(callback);
            EventBus<T>.Register(eventBinding);
        }
        private void UnRegisterEvent<T>(EventBinding<T> eventBinding) where T : IEvent
        {
            EventBus<T>.UnRegister(eventBinding);
        }

#region Builder

        public class Builder
        {
            PokemonBattleView _view;
            BattleType _type = BattleType.WildPokemon;

            List<PlayerPokemon> _playerPokemon;
            List<WildPokemon> _wildPokemon;

            public Builder(PokemonBattleView view) => _view = view;
            public Builder WithPokemon(List<PlayerPokemon> playerPokemon, List<WildPokemon> wildPokemon)
            {
                _playerPokemon = playerPokemon;
                _wildPokemon = wildPokemon;
                return this;
            }
            public Builder WithType(BattleType type)
            {
                _type = type;
                return this;
            }

            public PokemonBattleController Build()
            {
                PokemonBattleModel model = new(_playerPokemon, _wildPokemon);
                return new PokemonBattleController(_view, model, _type);
            }
        }

#endregion
    }
}