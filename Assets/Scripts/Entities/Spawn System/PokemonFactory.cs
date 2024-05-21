using UnityEngine;

namespace Pokemon
{
    public class PokemonFactory<T> : EntityFactory<T> where T : Pokemon
    {
        PokemonData[] _data;
        public PokemonFactory(PokemonData[] data) : base(data)
        {
            _data = data;
        }
        public override T Create(Transform spawnPoint)
        {
            PokemonData pokemonData = _data[Random.Range(0, _data.Length)];
            GameObject instance = GameObject.Instantiate(pokemonData._prefab, spawnPoint.position, spawnPoint.rotation);

            T pokemon = instance.GetComponent<T>();
            pokemon.SetBattleData(pokemonData);
            return pokemon;
        }
    }
}