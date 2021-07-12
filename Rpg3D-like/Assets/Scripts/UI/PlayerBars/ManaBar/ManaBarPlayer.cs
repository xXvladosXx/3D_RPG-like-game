using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;
using UnityEngine.UI;

public class ManaBarPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _manaBackground;
    [SerializeField] private Image _imageMana;

    private Mana _mana;
    private LevelUp _level;
    private GameObject _manaBar;
    private Image _imageManaModifier;
    private void Awake()
    {
        foreach (Transform mana in transform)
        {
            if(mana.CompareTag("UImana"))
                _manaBar = mana.gameObject;
        }
        
        _mana = GetComponent<Mana>();
        _level = GetComponent<LevelUp>();
        _imageManaModifier = _manaBackground.GetComponent<Image>();
    }

    private void Update()
    {
        SetManaBar();
    }

    private void SetManaBar()
    {
        _imageMana.fillAmount = _mana.GetFraction();
    }

   
}

