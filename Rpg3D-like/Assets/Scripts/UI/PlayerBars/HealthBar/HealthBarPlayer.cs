using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using Image = UnityEngine.UI.Image;

public class HealthBarPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _healthBackground;
    [SerializeField] private Image _imageHealth;

    private Health _health;
    private GameObject _healthBar;
    
    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _results;
    private FindStat _findStat;

    private void Awake()
    {
        foreach (Transform health in transform)
        {
            if(health.CompareTag("UI"))
                _healthBar = health.gameObject;
        }

        _findStat = GetComponent<FindStat>();
        _health = GetComponent<Health>();
        
        SetHealthBar();

        _health.OnTakeDamage += SetHealthBar;
        _health.OnTakeHealing += SetHealthBar;
        _findStat.OnLevelUp += SetHealthBar;
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    private void SetHealthBar(GameObject damager)
    {
        SetHealthBar();
    }
    private void SetHealthBar()
    {
        _imageHealth.fillAmount = _health.GetFraction();
    }
}
