using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Pokemon
{
    public class BattleUI: MonoBehaviour
    {
        [SerializeField] public ActivePokemonUI _playerPokemon;
        [SerializeField] public ActivePokemonUI _opponentPokemon;
        
        [SerializeField] private PartyPokemonUI[] _party;
        [SerializeField] public MoveUI[] _moves;
        
        [SerializeField] public DialougeUI _dialougeUI;
        [SerializeField] public CanvasGroup _cg;
        [SerializeField] public TMP_Text _dText;

        [SerializeField] private int _textSpeed = 25;

        [SerializeField] public CanvasGroup _partyMenu;
        [SerializeField] public CanvasGroup _battleMenu;
        [SerializeField] public CanvasGroup _movesMenu;
        [SerializeField] public CanvasGroup _lastUsed;

        public void UpdateActivePokemon(ActivePokemonUI pokemonUI, Pokemon pokemon)
        {
            pokemonUI.Update(pokemon._name, pokemon._level, pokemon.HP, pokemon._stats[StatType.HP]);
        }
        public async Task TypeDialogue(string message, int fadeDuration=1000)
        {
            _dialougeUI._canvasGroup.gameObject.SetActive(true);  
            _dialougeUI._canvasGroup.DOFade(1, 0.25f);

            _dialougeUI._text.text = "";

            foreach (char c in message.ToCharArray())
            {
                _dialougeUI._text.text += c;
                await Task.Delay(1000/_textSpeed);
            }

            await Task.Delay(fadeDuration);
            await _dialougeUI._canvasGroup.DOFade(0, 0.25f).AsyncWaitForCompletion();
            _dialougeUI._canvasGroup.gameObject.SetActive(false);
        }
        public void ClearMoves()
        {
            for (int i=0; i<(_moves.Length - 1); i++) _moves[i].Clear();
        }
        public void UpdateMove(Move move, MoveUI moveUI, Pokemon attacker, Pokemon defender)
        {
            moveUI.Update(move._moveData._name, move._movePP, move._moveData._pp);
            
            moveUI._button.enabled = true;
            moveUI._button.onClick.AddListener(() => MoveSelected(move, attacker, defender));
        }
        private void MoveSelected(Move move, Pokemon pokemon, Pokemon opponent)
        {
            EventBus<MoveUsedEvent>.Raise(new MoveUsedEvent
            {
                attacker = pokemon,
                move = move,
                defender = opponent,
            });

            SelectButton(_battleMenu.transform.GetChild(1).GetChild(0).gameObject);
        }
        private void UpdateParty(List<PlayerPokemon> party)
        {
            for (int i = 1; i < party.Count; i++)
            {
                var pokemon = party[i];
                var UI = _party[i];

                UI.Update(null, null, pokemon.HP, pokemon._stats[StatType.HP], pokemon._level);
            }
        }
        public async Task EnableMenu(CanvasGroup menu, bool enable = true)
        {
            if (enable == true) menu.gameObject.SetActive(enable);
            
            var fade = enable == true ? 1 : 0;
            await menu.DOFade(fade, 0.25f).SetEase(Ease.InOutSine).AsyncWaitForCompletion();

            if (enable == false) menu.gameObject.SetActive(enable);
        }
        public void UpdateHP(Pokemon pokemon, ActivePokemonUI pokemonUI)
        {
            pokemonUI._currentHP.text = pokemon.HP + "/" + pokemon._stats[StatType.HP];
            pokemonUI._healthBar.DOFillAmount((float)pokemon.HP/pokemon._stats[StatType.HP], 1f);
        }

        public async void Battle()
        {
            await Task.WhenAll
            (
                EnableMenu(_movesMenu),
                EnableMenu(_battleMenu, false)
            );

            SelectButton(_moves[0]._button.gameObject);
        }
        public async void Main()
        {
            await Task.WhenAll
            (
                EnableMenu(_battleMenu),
                EnableMenu(_movesMenu, false)
            );
            
            SelectButton(_battleMenu.transform.GetChild(1).GetChild(0).gameObject);
        }

        private void SelectButton(GameObject button)
        {
            EventSystem.current.SetSelectedGameObject(button);
        }

    }
}