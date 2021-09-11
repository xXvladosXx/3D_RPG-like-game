using Scriptable.Stats;
using Stats;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

namespace UI.PlayerBars.HealthBar
{
    public class HealthBarPlayer : MonoBehaviour
    {
        [SerializeField] private Image _imageHealth;

        private Health _health;
        private FindStat _findStat;

        private void Awake()
        {
            _findStat = GetComponentInParent<FindStat>();
            _health = GetComponentInParent<Health>();
            
            AddEvent(EventTriggerType.PointerEnter, delegate { OnEnter(); });
            AddEvent(EventTriggerType.PointerExit, delegate { OnExit(); });
            
            SetHealthBar();

            _health.OnHealthChanged += SetHealthBar;
            _findStat.OnLevelUp += SetHealthBar;
        }
    
        private void SetHealthBar()
        {
            _imageHealth.fillAmount = _health.GetFraction();
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
            Tooltip.EnableTooltip(_health.HealthCurrent + "\n" + _health.HealthMax);
        }

        private void OnExit()
        {
            Tooltip.DisableTooltip();
        }
    }
}
