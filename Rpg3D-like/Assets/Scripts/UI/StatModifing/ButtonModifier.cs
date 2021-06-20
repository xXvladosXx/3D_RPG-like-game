using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonModifier : MonoBehaviour
{
    public event UnityAction action;


    public void Add()
    {
        print("add");
    }

    public void Remove()
    {
        print("Remvoe");
    }
}
