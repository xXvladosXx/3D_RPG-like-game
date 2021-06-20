using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scriptable.Weapon
{
    [CreateAssetMenu(fileName = "FilteringByComponent", menuName = "Abilities/FilterComponent", order = 0)]
    public class TagFilter : FilterStrategy
    {
        [SerializeField] private String _tag;
        public override IEnumerable<GameObject> Filter(IEnumerable<GameObject> objectsToFilter)
        {
            foreach (var objectWithTag in objectsToFilter)
            {
                if (objectWithTag.CompareTag(_tag))
                {
                    yield return objectWithTag;
                }
            }
        }
    }
}