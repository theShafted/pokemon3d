using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon
{
    public class MoveView: View
    {
        public Button Button;
        public TMP_Text Name;
        public TMP_Text PP;
        public TMP_Text Effectiveness;

        public void Set(Move move, PokemonType[] types=null)
        {
            Name.text = move._moveData._name;
            PP.text = $"{move._movePP}/{move._moveData._pp}";

            if (types != null && move._moveData._category != MoveType.Status)
                Effectiveness.text = GetTypeEffectiveness(types, move._moveData._type);
            
            Button.enabled = true;
        }
        public void Clear()
        {
            Name.text = "";
            PP.text = "";
            Effectiveness.text = "";
            Button.enabled = false;
        }

        private string GetTypeEffectiveness(PokemonType[] pokemonTypes, PokemonType moveType)
        {
            float result = 1f;
            foreach (PokemonType type in pokemonTypes) result *= TypeEffectiveness.Value(moveType, type);

            return result switch
            {
                0f => "Ineffective",
                1f => "Effective",
                >= 2f => "Super Effective",
                _ => "Not Effective",
            };
        }
    }
}
