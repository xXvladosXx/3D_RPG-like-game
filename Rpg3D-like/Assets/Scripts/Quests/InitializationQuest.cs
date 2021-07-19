using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class InitializationQuest : ScriptableObject
{
    public abstract void InitQuest(GameObject questGiver, Action completed);
    public abstract GameObject GetAim();
}

public interface ICollactable
{
    public void Collect(GameObject item);
}

public interface IMoveable
{
    public event Action OnUpdateQuest;
    public void MoveTo(GameObject player, GameObject other);
}

public interface IKillable
{ 
   public void Kill(List<CombatTarget> combatTargets, int amount);
}
