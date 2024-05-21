using UnityEngine;

namespace Pokemon
{
    public class EntityFactory<T> : IEntityFactory<T> where T : Entity
    {
        EntityData[] _data;

        public EntityFactory(EntityData[] data)
        {
            _data = data;
        }
        public virtual T Create(Transform spawnPoint)
        {
            EntityData entityData = _data[Random.Range(0, _data.Length)];
            GameObject instance = GameObject.Instantiate(entityData._prefab, spawnPoint.position, spawnPoint.rotation);

            return instance.GetComponent<T>();
        }
    }
}