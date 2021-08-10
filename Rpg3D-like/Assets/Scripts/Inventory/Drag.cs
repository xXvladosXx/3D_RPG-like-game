using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventories
{    public class Drag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private Canvas _canvas;

        private RectTransform _rectTransform;
        [SerializeField] private Vector3 _defaultTransform;
        
        private CanvasGroup _canvasGroup;
        private GameObject _itemContainer;
        private GameObject _inventory;
        private GameObject _playerBar;

        private bool _isInSlot;

        public void SetInSlot(bool isInSlot)
        {
            _isInSlot = isInSlot;
        }


        private void Awake()
        {
            _canvas = GameObject.FindWithTag("MainCanvas").GetComponent<Canvas>();
            
            _rectTransform = GetComponent<RectTransform>();
           
            
            _canvasGroup = GetComponent<CanvasGroup>();
            
            _itemContainer = transform.parent.gameObject;
            _inventory = _itemContainer.transform.parent.gameObject;
            _playerBar = _inventory.transform.parent.gameObject;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _defaultTransform = transform.position;
            print(_defaultTransform);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isInSlot = false;
            _canvasGroup.alpha = .6f;
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            print(_isInSlot);
            print(_defaultTransform +  "+ defalut " );
            print(transform.position);
            
            if (_isInSlot == false)
                transform.position = _defaultTransform;
            
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor 
                                                               / _itemContainer.transform.localScale 
                                                               / _inventory.transform.localScale
                                                               / _playerBar.transform.localScale;
        }
    }
}