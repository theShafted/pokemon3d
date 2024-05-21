using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

namespace Pokemon
{
    public enum UIType
    {
        Main,
        Move,
        Party,
        Dialogue,
        LastUsed,
        Bag,
    }
    [Serializable] public class MoveUI
    {
        public Button _button;
        public TMP_Text _name;
        public TMP_Text _PP;
        public TMP_Text _effectiveness;

        public void Update(string name, int pp, int maxPP, string effectiveness="")
        {
            _name.text = name;
            _PP.text = pp + "/" + maxPP;
            _effectiveness.text = effectiveness;
            _button.enabled = true;
        }
        public void Clear()
        {
            _name.text = "-";
            _PP.text = "";
            _effectiveness.text = "";
            _button.enabled = false;
        }
    }
    [Serializable] public class PartyPokemonUI
    {
        public Button _button;
        public Image _icon;
        public Image _item;
        public Image _healthBar;
        public TMP_Text _HP;
        public TMP_Text _level;

        public void Update(Image icon, Image item, int HP, int maxHP, int level)
        {
            _icon = icon;
            _item = item;
            _healthBar.fillAmount = HP/maxHP;
            _HP.text = HP + "/" + maxHP;
            _level.text = level.ToString();
        }
    }
    [Serializable] public class ActivePokemonUI
    {
        public CanvasGroup _canvasGroup;
        public TMP_Text _name;
        public TMP_Text _level;
        public TMP_Text _currentHP;
        public Image _healthBar;
        public List<Image> _statModifiers = new(5);

        public void Update(string name, int level, int HP, int maxHP, bool reset=true)
        {
            _name.text = name;
            _level.text = "Lv. " + level;
            _currentHP.text = HP + "/" + maxHP;
            _healthBar.fillAmount = (float)HP/maxHP;

            if (reset) foreach (Image image in _statModifiers) image?.gameObject.SetActive(false);
        }
        public void UpdateStat(StatType stat, int stage=1 )
        {
            var statUI = _statModifiers[(int)stat - 1];
            bool enable = stage != 0;
            
            statUI.gameObject.SetActive(enable);
            if (enable) statUI.color = stage > 0 ? Color.cyan : Color.red;
        }
    }
    [Serializable] public class DialougeUI
    {
        public CanvasGroup _canvasGroup;
        public TMP_Text _text;
    }
}