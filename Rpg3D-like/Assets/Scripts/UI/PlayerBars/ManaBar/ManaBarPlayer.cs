using Stats;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.PlayerBars.ManaBar
{
    public class ManaBarPlayer : MonoBehaviour
    {
        [SerializeField] private Image _imageMana;

        private Mana _mana;
        private void Awake()
        {
            _mana = GetComponentInParent<Mana>();
            
            AddEvent(EventTriggerType.PointerEnter, delegate { OnEnter(); });
            AddEvent(EventTriggerType.PointerExit, delegate { OnExit(); });
        }

        private void Update()
        {
            SetManaBar();
        }

        private void SetManaBar()
        {
            _imageMana.fillAmount = _mana.GetFraction();
        }

        private void AddEvent(EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
            var eventTrigger = new EventTrigger.Entry {eventID = type};
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }
        
        private void OnEnter()
        {
            Tooltip.EnableTooltip(_mana.ManaCurrent + "\n" + _mana.ManaMax);
        }

        private void OnExit()
        {
            Tooltip.DisableTooltip();
        }
    }
}

