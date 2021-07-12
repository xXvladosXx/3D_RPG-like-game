using System;
using System.Collections.Generic;
using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SkillBar
{
    public class SkillBarPlayer : MonoBehaviour
    {
        [SerializeField] private Dictionary<int, GameObject> _childSkillsList;

        private int _skillIndex = 0;
        private void Awake()
        {
            _childSkillsList = new Dictionary<int, GameObject>();

            foreach (Transform child in transform)
            {
                _childSkillsList.Add(_skillIndex, child.gameObject);
                _skillIndex++;
            }
        }

        public void TriggerCastingSkill(int index)
        {
            foreach (var skillBox in _childSkillsList)
            {
                if (skillBox.Key == index)
                {
                    skillBox.Value.GetComponent<SkillBox>().SetCasted(true);
                }
            }
        }
    }
}