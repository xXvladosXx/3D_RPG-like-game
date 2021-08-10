using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InitializeDialogueStrategy : MonoBehaviour
{
    public abstract event Action OnDialogChanged;
    public abstract void InitializeDialogMessage();
}

public interface ITalkable
{
    public void Talk();
}

public interface IUpgradeable
{
    public void Upgrade();
}

public interface ITransactable
{
    public void Transact();
}