using UnityEngine;
using UnityEngine.UI;

namespace UI.PlayerBars.HealthBar
{
    public class HealthBarCompanion : MonoBehaviour
    {
        [SerializeField] private Image _imageHealthForeground;
        [SerializeField] private FriendlyAIController _companion;
    
        private Health _health;
        private void OnEnable()
        {
            _companion = FindObjectOfType<FriendlyAIController>();
            
            _health = _companion.GetComponent<Health>();
            _health.OnTakeDamage += HealthBarMaintenance;
            _health.OnTakeHealing += HealthBarMaintenance;
            _companion.GetComponent<FindStat>().OnLevelUp += HealthBarMaintenance;
        }

        private void Start()
        {
            _imageHealthForeground.fillAmount =_health.GetFraction();
        }

        private void HealthBarMaintenance(GameObject damager)
        {
            HealthBarMaintenance();
        }
    
        private void HealthBarMaintenance()
        {
            _imageHealthForeground.fillAmount = _health.GetFraction();
        }
    }
}