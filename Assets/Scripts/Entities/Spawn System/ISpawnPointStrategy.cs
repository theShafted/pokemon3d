using UnityEngine;

namespace Pokemon
{
    public interface ISpawnPointStrategy
    {
        Transform NextSpawnPoint();
    }
}