using System.Collections;
using System.Collections.Generic;
using Pokemon;
using UnityEngine;

namespace Pokemon
{
    public class Move
    {
        public MoveData _moveData;
        public int _movePP;
        
        public Move (MoveData moveData)
        {
            _moveData = moveData;
            _movePP = _moveData._pp;
        }
    }
}