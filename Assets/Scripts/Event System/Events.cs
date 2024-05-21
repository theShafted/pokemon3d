using System.Collections.Generic;

namespace Pokemon
{
    public enum BattleType
    {
        WildPokemon,
        Trainer
    }

    public interface IEvent { }

    public struct TestEvent: IEvent {}

    public struct BattleStartEvent: IEvent
    {
        public BattleType _type;
        public List<WildPokemon> _opponentTeam;
        public List<PlayerPokemon> _playerTeam;
    }
    public struct BattleMenuEvent: IEvent { public BattleAction Action; }
    public struct MovesMenuEvent: IEvent { public int Index; }
    public struct PokemonChangeEvent: IEvent { public int Index; }
    public struct MoveUsedEvent: IEvent
    {
        public Pokemon attacker;
        public Move move;
        public Pokemon defender;
    }
    public struct DialogueEvent: IEvent
    {
        public string message;
    }
    public struct BattleOverEvent: IEvent {}
}