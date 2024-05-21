using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon
{
    public enum StatType
    {
        HP,
        Attack,
        Defense,
        Speed,
        SpecialAttack,
        SpecialDefense,
    }
    public enum PokemonType
    {
        None,
        Normal,
        Flying,
        Poison,
        Ghost,
        Dark
    }

    public class DamageData
    {
        public int _damage;
        public bool _fainted;
        public bool _critical;
        public float _type;
    }

    public class StatModifiers
    {
        public static readonly Dictionary<int, float> _values = new(13)
        {
            [-6] = 2/8f,
            [-5] = 2/7f,
            [-4] = 2/6f,
            [-3] = 2/5f,
            [-2] = 2/4f,
            [-1] = 2/3f,
            [00] = 2/2f,
            [01] = 3/2f,
            [02] = 4/2f,
            [03] = 5/2f,
            [04] = 6/2f,
            [05] = 7/2f,
            [06] = 8/2f 
        };
    }
    public class TypeEffectiveness
    {
        static readonly float[][] Chart = 
        {
            //          Normal Flying Poison Ghost Dark
            new float[] {1.0f, 1.0f, 1.0f, 0.0f, 1.0f},    // Normal
            new float[] {1.0f, 1.0f, 1.0f, 1.0f, 1.0f},    // Flying   
            new float[] {1.0f, 1.0f, 0.5f, 0.5f, 1.0f},    // Poison
            new float[] {0.0f, 1.0f, 1.0f, 2.0f, 0.5f},    // Ghost
            new float[] {1.0f, 1.0f, 1.0f, 2.0f, 0.5f},    // Dark
        };

        public static float Value(PokemonType attack, PokemonType defense)
        {
            if (attack == PokemonType.None || defense == PokemonType.None) return 1.0f;

            return Chart[(int)attack - 1][(int)defense - 1]; 
        }
    }

    [Serializable] public class Stat
    {
        public StatType _stat;
        public int _value;

        public Stat(StatType stat, int value)
        {
            _stat = stat;
            _value = value;
        } 
    }

    [Serializable] public class LearnableMove
    {
        public MoveData _move;
        public int _level;
    }

    [CreateAssetMenu(fileName = "PokemonData", menuName = "Pokemon/Pokemon Data")]
    public class PokemonData: EntityData
    {
        public string _name;
        public PokemonType[] _types;
        public List<Stat> _baseStats = new(4);
        public List<LearnableMove> _learnableMoves;

        [ContextMenu("Populate Stats")]
        public void PopulateStats()
        {
            for (int i = 0; i < 6; i++) _baseStats.Add(new Stat((StatType)i, 0));
        }
        
    }
}