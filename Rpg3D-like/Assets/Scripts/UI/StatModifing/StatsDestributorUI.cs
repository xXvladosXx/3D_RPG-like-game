using System;
using Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.StatModifing
{
    public class StatsDestributorUI : MonoBehaviour
    {
        [SerializeField] private StatsEnum _stat;
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private Button _minus;
        [SerializeField] private Button _plus;

        private StatsValueStore _statsValue;
       
        private void Start()
        {
            _statsValue = GameObject.FindGameObjectWithTag("Player").GetComponent<StatsValueStore>();
            
            _plus.onClick.AddListener((() => {Allocate(1);}));
            _minus.onClick.AddListener((() => {Allocate(-1);}));
            
        }

        private void Update()
        {
            _minus.interactable = _statsValue.CanAssignPoints(_stat, -1);
            _plus.interactable = _statsValue.CanAssignPoints(_stat, 1);

            _valueText.text = _statsValue.GetProposedPoints(_stat).ToString();
        }

        public void Allocate(int points)
        {
           _statsValue.AssignPoints(_stat, points);
        }
    }
}