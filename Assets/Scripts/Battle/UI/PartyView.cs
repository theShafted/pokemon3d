using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pokemon
{

    public class PartyView: View
    {
        public List<PartyPokemonView> PartyPokemon;

        protected override async Task OnEnable()
        {
            for (int i = 0; i < PartyPokemon.Count; i++)
            {
                int index = i;
                PartyPokemon[i].Button.onClick.AddListener(() => OnClick(index));
            }

            await base.OnEnable();
        }

        public void Set(List<PlayerPokemon> pokemon)
        {
            for (int i = 1; i<pokemon.Count; i++) PartyPokemon[i].Set(pokemon[i]);
        }
        public void Clear()
        {
            foreach (var pokemon in PartyPokemon) pokemon.Clear();
        }

        private void OnClick(int index)
        {
            gameObject.SetActive(false);
            EventBus<PokemonChangeEvent>.Raise(new PokemonChangeEvent { Index = index });
        }
    }
}
