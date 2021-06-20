﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable.Weapon
{
    public class SkillData
    {
        private GameObject _user;
        public GameObject GetUser => _user;

        public void SetUser(GameObject user)
        {
            _user = user;
        }
        
        private IEnumerable<GameObject> _targets;
        public IEnumerable<GameObject> GetTargets => _targets;
        public void SetTargets(IEnumerable<GameObject> targets)
        {
            _targets = targets;
        }

        private Vector3 _mousePosition;
        public Vector3 GetMousePosition => _mousePosition;

        public void SetMousePosition(Vector3 mousePosition)
        {
            _mousePosition = new Vector3(mousePosition.x, mousePosition.y, mousePosition.z);
        }
        
        public SkillData(GameObject user)
        {
            _user = user;
        }

        public void StartCoroutine(IEnumerator coroutine)
        {
            GetUser.GetComponent<MonoBehaviour>().StartCoroutine(coroutine);
        }
    }
}