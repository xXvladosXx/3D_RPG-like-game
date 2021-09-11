using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] private FindStat _findStat;
    public FindStat GetCharacterStat => _findStat;
    [SerializeField] private TextMeshProUGUI _mana;
    [SerializeField] private TextMeshProUGUI _health;
    [SerializeField] private TextMeshProUGUI _damage;
    
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetCharacterInfo(FindStat stat)
    {
        _findStat = stat;
        _animator.enabled = true;        
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        _mana.text = _findStat.GetStat(StatsEnum.Mana).ToString();
        _health.text = _findStat.GetStat(StatsEnum.Health).ToString();
        _damage.text = _findStat.GetStat(StatsEnum.Damage).ToString();

        StartCoroutine(DisableAnimator());
    }

    private IEnumerator DisableAnimator()
    {
        yield return new WaitForSeconds(1.3f);
        _animator.enabled = false;
    }
}
