using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Inventory;
using UI.Cursor;
using UnityEngine;

public class ChestOpener : MonoBehaviour, IRaycastable
{
    [SerializeField] private float _distanceToOpen;
    [SerializeField] private PlayerController _player;
    [SerializeField] private float _dropForwardForce, _dropUpwardForce;
    [SerializeField] private ItemTrigger[] _items;
    [SerializeField] private Transform _pointToOpen;
    
    private Animator _animation;
    private bool _wasOpened;

    private void Awake()
    {
        _animation = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if(_wasOpened) return;
        
        if (Vector3.Distance(_player.transform.position, gameObject.transform.position) < _distanceToOpen)
        {
            _animation.enabled = true;
            Open();
        }
    }

    private void Open()
    {
        _wasOpened = true;
        
        StartCoroutine(SpawnObject());
    }

    private IEnumerator SpawnObject()
    {
        float angle = 1f;
        
        foreach (var item in _items)
        {
            yield return new WaitForSeconds(.5f);

            var itemToInstantiate = Instantiate(item, transform);
        
            itemToInstantiate.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity;
            
            itemToInstantiate.GetComponent<Rigidbody>().AddForce(gameObject.transform.up * _dropUpwardForce *angle, ForceMode.Impulse);

            angle += 1;
        }
    }

    public PlayerController.CursorType GetCursorType()
    {
        return PlayerController.CursorType.PickUp;
    }

    public bool HandleRaycast(PlayerController player)
    {
        if (Input.GetMouseButton(0))
        {
            player.GetComponent<Movement>().StartMoveToAction(_pointToOpen.position, 1f);
        }
        return true;
    }
}
