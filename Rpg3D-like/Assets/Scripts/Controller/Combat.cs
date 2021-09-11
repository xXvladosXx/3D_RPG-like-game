using Scriptable.Weapon;
using Stats;
using UnityEngine;

namespace Controller
{
    public class Combat : MonoBehaviour, IAction
    {
        private Animator _animator;
        private float _attackCooldown = 0;
        private Transform _target;
        private ActionScheduler _actionScheduler;
        private Equipment _equipment;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _equipment = GetComponent<Equipment>();
        }
        

        void Update()
        {
            if(_target == null) return;
            if (_target.GetComponent<Health>().IsDead())
            {
                _target = FindNewTarget();
                if(_target == null) return;
            }

            bool isInRange = GetDistance();
            if(isInRange)
                AttackBehaviour();
            
            if (_attackCooldown > 0)
            {
                _attackCooldown -= Time.deltaTime;
            }
        }

        private Transform FindNewTarget()
        {
            CombatTarget[] targets = FindObjectsOfType<CombatTarget>();
            float minDistance = 3f;

            foreach (CombatTarget target in targets)
            {
                if(target.GetComponent<Health>().IsDead()) continue;
            
                float distanceToTarget = Vector3.Distance(gameObject.transform.position, target.transform.position);

                if (minDistance > distanceToTarget)
                {
                    minDistance = distanceToTarget;
                    return target.transform;
                }
            }

            return null;
        }

        private void AttackBehaviour()
        {
            if (_attackCooldown <= 0)
                TriggerStartAttack();
        }


        private bool GetDistance()
        {
            return Vector3.Distance(gameObject.transform.position, _target.transform.position) < _equipment.GetCurrentWeapon.GetAttackRange;
        }

        public bool CanAttack(GameObject target)
        {
            if (target.GetComponent<Health>() == null) return false;
            if (target.GetComponent<Health>().IsDead())
            {
                Cancel();

                return false;
            }

            Health targetHealth = target.GetComponent<Health>();

            return targetHealth != null;
        }
    
        public void Attack(Transform target)
        {
            if(target.GetComponent<Health>().IsDead()) return;
        
            _actionScheduler.StartAction(this);

            _target = target.transform;
            transform.LookAt(_target);
        }

        private void TriggerStartAttack()
        { 
            if(_target.GetComponent<Health>() ==null) return;
            if(_target.GetComponent<Health>().IsDead()) return;
        
            _attackCooldown = _equipment.GetCurrentWeapon.GetAttackSpeed;

            _animator.ResetTrigger("stopAttack");
            _animator.SetTrigger("attack");
        }

        private void TriggerInterruptAttack()
        {
            _animator.ResetTrigger("attack");
            _animator.SetTrigger("stopAttack");
        }
       

        public void Cancel()
        {
            TriggerInterruptAttack();
        
            _target = null;
        }
        
        void Hit()
        {
            if(_target == null) return;

            if(_target.GetComponent<Health>() == null) return;  
     
            transform.LookAt(_target);
            _target.GetComponent<Health>().TakeDamage(_equipment.GetCurrentWeapon._damage, gameObject);
        }

        void Shoot()
        {
            if(_target == null) return;
            if(_equipment.GetCurrentWeapon.GetProjectile() == null) return;
        
            transform.LookAt(_target);
            _equipment.GetCurrentWeapon.SpawnProjectile(_target, gameObject.transform, _equipment.GetCurrentWeapon._damage);
        }
    
        void FootR()
        {
        
        }
    
        void FootL()
        {
        
        }
    }
}
