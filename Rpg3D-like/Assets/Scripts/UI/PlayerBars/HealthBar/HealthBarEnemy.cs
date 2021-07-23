using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarEnemy : MonoBehaviour
{
    [SerializeField] private Image _imageHealthForeground;
    [SerializeField] private GameObject _healthBar;
    [SerializeField] private GameObject _enemy;
    
    private Health _health;
    private void Awake()
    {
        _health = _enemy.GetComponent<Health>();

        _health.OnTakeDamage += HealthBarMaintenance;
        _health.OnTakeHealing += HealthBarMaintenance;
        _health.OnDied += () =>
        {
            _healthBar.SetActive(false);
        };
    }

    private void Start()
    {
        _imageHealthForeground.fillAmount =_health.GetFraction();
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }

    private void HealthBarMaintenance(GameObject damager)
    {
        HealthBarMaintenance();
    }
    
    private void HealthBarMaintenance()
    {
        _healthBar.SetActive(true);
        _imageHealthForeground.fillAmount = _health.GetFraction();
    }
    
}
