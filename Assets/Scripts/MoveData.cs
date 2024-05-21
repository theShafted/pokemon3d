using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon
{
    [CreateAssetMenu(fileName = "MoveData", menuName = "Pokemon/Move Data")]
    public class MoveData: ScriptableObject
    {
        public string _name;
        public int _power;
        public PokemonType _type;
        public MoveType _category;
        public int _accuracy;
        public int _pp;
        public MoveEffect _effect;
        public MoveTarget _target;
        
        [TextArea] public string _desctiption;

        public bool Physical() => _category == MoveType.Physical;
    }
    
    public enum MoveType
    {
        Physical,
        Special,
        Status
    }
    public enum MoveTarget
    {
        Self,
        Opponent
    }


    [Serializable] public class MoveEffect
    {
        public List<Stat> _stats;
        public MoveTarget _target;
    }
}
