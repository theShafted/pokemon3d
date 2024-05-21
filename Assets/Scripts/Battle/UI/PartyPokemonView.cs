using TMPro;
using UnityEngine.UI;

namespace Pokemon
{
    public class PartyPokemonView: View
    {
        public Button Button;
        public Image Icon;
        public TMP_Text HP;
        public TMP_Text Level;
        public Image Item;
        public Image Healthbar;

        public void Set(Pokemon pokemon)
        {
            if (pokemon == null) Clear();

            HP.text = $"{pokemon.HP}/{pokemon.GetStat(StatType.HP)}";
            Level.text = $"Lv. {pokemon._level}";
            Healthbar.fillAmount = (float)pokemon.HP / pokemon.GetStat(StatType.HP);
            Button.enabled = true;
        }
        public void Clear()
        {
            HP.text = "";
            Level.text = "";
            Healthbar.fillAmount = 0f;
            Button.enabled = false;
        }
    }
}
