using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Objects.Bosses
{
    public class PlayerChecker : BossAction
    {
        public float Cooldown;
        public float DistanceToTeleport = 5f;

        public override TaskStatus OnUpdate()
        {
            if (_health.IsDead()) return TaskStatus.Failure;

            if (Vector3.Distance(_playerController.transform.position, transform.position) < DistanceToTeleport)
            {
                _bossSkills.CastSkill(2);

                return TaskStatus.Success;
            }

            return TaskStatus.Inactive;
        }
    }
}