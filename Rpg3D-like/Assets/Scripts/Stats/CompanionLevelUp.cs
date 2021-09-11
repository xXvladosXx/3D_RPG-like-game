using System;
using Controller;

namespace Stats
{
    public class CompanionLevelUp : LevelUp
    {
        private LevelUp _player;
        private void Awake()
        {
            _player = FindObjectOfType<PlayerController>().GetComponent<LevelUp>();
        }

        public override void ExperienceReward(float experience)
        {
            _player.ExperienceReward(experience);
        }
    }
}