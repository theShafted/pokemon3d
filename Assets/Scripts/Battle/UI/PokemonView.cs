using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon
{
    public class PokemonView: View
    {
        public TMP_Text Name;
        public TMP_Text Level;
        public TMP_Text HP;
        public Image Healthbar;
        public List<Image> StatModifiers;

        public void Set(Pokemon pokemon)
        {
            Name.text = pokemon._name;
            Level.text = $"Lv. {pokemon._level}";
            HP.text = $"{pokemon.HP}/{pokemon._stats[StatType.HP]}";

            Healthbar.fillAmount = (float)pokemon.HP/pokemon._stats[StatType.HP];

            foreach (var stat in pokemon._statStages)
            {
                if (stat.Key == StatType.HP) continue;
                StatModifiers[(int)stat.Key - 1].gameObject.SetActive(stat.Value != 0);  
            }
        }
        public async Task UpdateHP(int hp, int maxHP)
        {
            HP.text = $"{hp}/{maxHP}";
            await Healthbar.DOFillAmount((float)hp/maxHP, 0.25f).AsyncWaitForCompletion();
        }
        public void Remove() => _canvasGroup.DOFade(0f, 0.25f).SetEase(Ease.InOutExpo);

    }
}
