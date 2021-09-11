using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Controller;
using DefaultNamespace.Objects.Bosses;
using Scriptable.Weapon.SkillsSpecification;
using Stats;
using UI.PlayerBars.SkillBar;
using UnityEngine;
using UnityEngine.AI;

public class BossAction : Action
{
    protected PlayerController _playerController;
    protected NavMeshAgent _navMeshAgent;
    protected Animator _animator;
    protected Rigidbody _rigidbody;
    protected Movement _movement;
    protected Equipment _equipment;
    protected BossSkills _bossSkills;
    protected CooldownSkillManager _cooldownSkillManager;
    protected Health _health;
    
    public override void OnAwake()
    {
        _playerController = Object.FindObjectOfType<PlayerController>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _movement = GetComponent<Movement>();
        _equipment = GetComponent<Equipment>();
        _bossSkills = GetComponent<BossSkills>();
        _cooldownSkillManager = GetComponent<CooldownSkillManager>();
        _health = GetComponent<Health>();
    }

}
