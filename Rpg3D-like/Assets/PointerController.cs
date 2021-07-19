using System;
using System.Collections;
using System.Collections.Generic;
using Quests;
using UnityEngine;

public class PointerController : MonoBehaviour
{
   [SerializeField] private QuestSystem _questSystem;

   private void Update()
   {
      if(_questSystem.GetQuest == null) return;
      
      transform.LookAt(_questSystem.GetQuest.GetCurrentQuest.GetAim().transform);
   }
}
