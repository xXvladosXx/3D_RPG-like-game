using System;
using Controller;
using Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.StatModifing
{
    public class StatsConfirmUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private Button _confirmButton;

        private StatsValueStore _statsValue;

        private void Awake()
        {
            _statsValue = FindObjectOfType<PlayerController>().GetComponent<StatsValueStore>();
        }

        private void Start()
        {
            _confirmButton.onClick.AddListener( _statsValue.Confirm );
        }


        private void Update()
        {
            _valueText.text = _statsValue.GetUnassignedPoints.ToString();
        }
    }
}