using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pokemon
{
    public abstract class View: MonoBehaviour
    {
        [SerializeField] protected CanvasGroup _canvasGroup;

        protected virtual async Task OnEnable() => await InitializeView(); 
        protected virtual async Task OnDisable() => await DisableView();

        protected void SetElement(GameObject element) => EventSystem.current.SetSelectedGameObject(element);

        public virtual async Task InitializeView()
        {
            await _canvasGroup.DOFade(1f, 0.25f).SetEase(Ease.InOutExpo).AsyncWaitForCompletion();
        }
        private async Task DisableView()
        {
            await _canvasGroup.DOFade(0f, 0.25f).SetEase(Ease.InOutExpo).AsyncWaitForCompletion();
        }
        
    }
}
