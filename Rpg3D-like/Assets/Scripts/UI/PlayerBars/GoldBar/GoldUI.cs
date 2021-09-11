using Inventory;
using TMPro;
using UnityEngine;

namespace UI.PlayerBars.GoldBar
{
    public class GoldUI : MonoBehaviour
    {
        [SerializeField] private Gold _playerGold;
        [SerializeField] private TextMeshProUGUI _goldAmount;
    
        private void Start()
        {
            PlayerGoldOnOnGoldChanged();
        
            if(_playerGold!=null)
                _playerGold.OnGoldChanged += PlayerGoldOnOnGoldChanged;
        }

        private void PlayerGoldOnOnGoldChanged()
        {
            _goldAmount.text = _playerGold.GetGold.ToString();
        }
    }
}
