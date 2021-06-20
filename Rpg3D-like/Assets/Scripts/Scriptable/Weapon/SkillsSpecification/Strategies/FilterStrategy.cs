using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable.Weapon
{
    public abstract class FilterStrategy : ScriptableObject
    {
        public abstract IEnumerable<GameObject> Filter(IEnumerable<GameObject> objectsToFilter);
    }
}