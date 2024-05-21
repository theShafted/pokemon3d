using DG.Tweening;
using UnityEngine;

namespace Pokemon
{
    
    public class SpawnEffect: MonoBehaviour
    {
        [SerializeField] private GameObject _spawnFX;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private AudioSource _audio;

        void Start()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, _duration).SetEase(Ease.OutBack);

            if (_spawnFX != null)
            {
                Instantiate(_spawnFX, transform.position, Quaternion.identity);
            }

            if (_audio != null)
            {
                _audio.Play();
            }
        }
    }
}