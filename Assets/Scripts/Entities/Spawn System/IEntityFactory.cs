using UnityEngine;

namespace Pokemon
{
    public interface IEntityFactory<T> where T: Entity
    {
        T Create(Transform spawnPoint);
    }
}