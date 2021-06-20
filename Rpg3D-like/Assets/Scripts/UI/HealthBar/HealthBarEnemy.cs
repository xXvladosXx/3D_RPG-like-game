using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarEnemy : MonoBehaviour
{
    [SerializeField] private Image _imageHealth;

    private Health _health;
    private HealthBarEnemy _healthBarEnemy;
    private GameObject _healthBar;
    private void Awake()
    {
        _health = GetComponent<Health>();
        _healthBarEnemy = GetComponentInChildren<HealthBarEnemy>();

        foreach (Transform health in transform)
        {
            if(health.CompareTag("UI"))
                _healthBar = health.gameObject;
        }
        
        _health.OnTakeDamage += HealthBarMaintenance;
        _health.OnTakeHealing += HealthBarMaintenance;
    }

    private void Start()
    {
        _imageHealth.fillAmount = GetComponent<Health>().GetFraction();
    }

    private void Update()
    {
        _healthBar.transform.LookAt(Camera.main.transform);
    }

    private void HealthBarMaintenance()
    {
        _imageHealth.fillAmount = _health.GetFraction();

        _healthBar.SetActive(!_health.IsDead());
    }
    
}
