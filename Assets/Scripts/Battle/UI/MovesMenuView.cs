using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Pokemon
{
    public class MovesMenuView: View
    {
        public List<MoveView> Moves;

        protected override async Task OnEnable()
        {
            for (int i = 0; i < Moves.Count - 1; i++)
            {
                int index = i;
                Moves[i].Button.onClick.AddListener(() => OnClick(index));
            }

            SetElement(Moves[0].gameObject);

            await base.OnEnable();
        }

        public void Set(List<Move> moves, PokemonType[] types)
        {
            for (int i = 0; i < moves.Count; i++)
            {
                Moves[i].Clear();
                Moves[i].Set(moves[i], types);
            }
        }
    
        private void OnClick(int index)
        {
            gameObject.SetActive(false);
            EventBus<MovesMenuEvent>.Raise(new MovesMenuEvent { Index = index });
        }
    }
}
