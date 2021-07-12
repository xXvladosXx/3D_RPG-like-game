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
    private LevelUp _level;
    private GameObject _healthBar;
    private Image _imageHealthModifier;
    
    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _results;
    private void Awake()
    {
        foreach (Transform health in transform)
        {
            if(health.CompareTag("UI"))
                _healthBar = health.gameObject;
        }
        
        _health = GetComponent<Health>();
        _level = GetComponent<LevelUp>();
        _imageHealthModifier = _healthBackground.GetComponent<Image>();
        
        SetHealthBar();
        
        _health.OnTakeDamage += SetHealthBar;
        _health.OnTakeHealing += SetHealthBar;
    }


    private void Update()
    {
        // IsPointerOverUIObject();
    }

    private void SetHealthBar()
    {
        _imageHealth.fillAmount = _health.GetFraction();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.CompareTag("UI"))
        {
            Debug.Log("Mouse Over: " + eventData.pointerCurrentRaycast.gameObject.name);
        }
    }
}
