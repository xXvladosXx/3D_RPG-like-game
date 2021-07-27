using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InitializeDialogueStrategy : MonoBehaviour
{
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
