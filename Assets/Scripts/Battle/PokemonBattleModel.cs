using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering;

namespace Pokemon
{
    public class PokemonBattleModel
    {
        public ObservableCollection<PlayerPokemon> Player { get; set; }
        public ObservableCollection<WildPokemon> Opponent { get; set; }

        public List<Move> PlayerMoves;
        public List<Move> OpponentMoves;

        // public event NotifyCollectionChangedEventHandler OnModelChanged
        // {
        //     add
        //     {
        //         foreach (var pokemon in Player) pokemon.PropertyChanged += value
        //     }
        //     remove => Player.CollectionChanged -= value;
            
        // }
        public event PropertyChangedEventHandler OnPropertyChanged
        {
            add
            {
                foreach (var pokemon in Player) pokemon.PropertyChanged += value;
            }
            remove
            {
                foreach (var pokemon in Player) pokemon.PropertyChanged -= value;
            }
        }

        public PokemonBattleModel(List<PlayerPokemon> playerPokemon, List<WildPokemon> opponentPokemon)
        {
            Player = new ObservableCollection<PlayerPokemon>();
            foreach (var pokemon in playerPokemon) Player.Add(pokemon);

            Opponent = new ObservableCollection<WildPokemon>();
            foreach (var pokemon in opponentPokemon) Opponent.Add(pokemon);
            
            PlayerMoves = GetPlayerPokemon()._moves;
            OpponentMoves = GetOpponentPokemon()._moves;
        }

        public Pokemon GetPlayerPokemon() => Player[0];
        public void PlayerSwitch(int source, int target)
        {
            (Player[source], Player[target]) = (Player[target], Player[source]);
        }
        
        public Pokemon GetOpponentPokemon() => Opponent[0];
        public void OpponentSwitch(int source, int target)
        {
            (Opponent[source], Opponent[target]) = (Opponent[target], Opponent[source]);
        }

    }
}