using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pokemon
{
    public class BattleMenuView: View
    {
        public List<Button> Buttons;

        protected override async Task OnEnable()
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                int index = i;
                Buttons[i].onClick.AddListener(() => OnClick(index));
            }

            SetElement(Buttons[0].gameObject);

            await base.OnEnable();
        }

        private void OnClick(int index)
        {
            gameObject.SetActive(false);
            EventBus<BattleMenuEvent>.Raise(new BattleMenuEvent
            {
                Action = (BattleAction)index
            });
        }
    }

    public enum BattleAction
    {
        Battle,
        Bag,
        Run,
        LastUsed,
    }
}
