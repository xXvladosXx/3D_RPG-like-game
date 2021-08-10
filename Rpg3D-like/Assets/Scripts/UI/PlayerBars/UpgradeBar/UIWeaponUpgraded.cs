using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PlayerBars.UpgradeBar
{
    public class UIWeaponUpgraded : MonoBehaviour
    {
        [SerializeField] private Image _weaponImage;
        [SerializeField] private TextMeshProUGUI _weaponLevel;
        [SerializeField] private bool _upgradeResult;

        public void SetWeaponSprite(Sprite weaponSprite)
        {
            _weaponImage.sprite = weaponSprite;
        }

        public void SetWeaponLevelText(string weaponLevel)
        {
            _weaponLevel.text = weaponLevel;
        }
    }

}
