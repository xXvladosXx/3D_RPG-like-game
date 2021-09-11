using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;

namespace DefaultNamespace.Objects.Bosses.FirstSkill
{
    public class SecondSkillAttack : BossAction
    {
        public int SpawnCount = 4;
        public float SpawnInterval = 0.8f;


        public override TaskStatus OnUpdate()
        {
            if (_health.IsDead()) return TaskStatus.Failure;
            
            var sequence = DOTween.Sequence();

            for (int i = 0; i < SpawnCount; i++)
            {
                sequence.AppendCallback(CastSkill);
                sequence.AppendInterval(SpawnInterval);
            }

            return TaskStatus.Success;
        }

        private void CastSkill()
        {
            if (_cooldownSkillManager.GetCooldownSkill(_bossSkills.GetSkillOnIndex(1)) <= 0)
                _bossSkills.CastSkill(1);
        }
    }
}