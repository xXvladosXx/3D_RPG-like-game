using Controller;
using Scriptable.Stats;
using UnityEngine;

namespace UI.PlayerBars.SkillBar
{
    public class SkillBarController : MonoBehaviour
    {
        [SerializeField] private int _pointsToUpgradeSkill = 1;
        public int GetPoints => _pointsToUpgradeSkill;
        private FindStat _playerLevel;
        private void Awake()
        {
            _playerLevel = FindObjectOfType<PlayerController>().GetComponent<FindStat>();
      
            _playerLevel.OnLevelUp += () => _pointsToUpgradeSkill++;
        }
        public void DistributePoint()
        {
            _pointsToUpgradeSkill--;
        }
    }
}
