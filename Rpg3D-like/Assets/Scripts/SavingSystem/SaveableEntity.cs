using System;
using System.Collections;
using System.Collections.Generic;
using Saving;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AI;

[ExecuteAlways]
public class SaveableEntity : MonoBehaviour
{
   [SerializeField] private string _uniqueIdentifier = System.Guid.NewGuid().ToString();
   private static Dictionary<string, SaveableEntity> globalLookThrough = new Dictionary<string, SaveableEntity>();
   public string GetUniqueIdentifier()
   {
      return _uniqueIdentifier;
   }

   public object CaptureState()
   {
      Dictionary<string, object> state = new Dictionary<string, object>();
      
      foreach (var saveable in GetComponents<ISaveable>())
      {
         state[saveable.GetType().ToString()] = saveable.CaptureState();
      }

      return state;
   }
   
   public void RestoreState(object state)
   {
      Dictionary<string, object> restoredState = state as Dictionary<string, object>;

      foreach (var saveable in GetComponents<ISaveable>())
      {
         string saveableSerialize = saveable.GetType().ToString();
         if (state is Dictionary<string,object> records)
         {
            saveable.RestoreState(restoredState[saveableSerialize]);
         }
      }
   }

#if UNITY_EDITOR
   private void Update()
   {
      if(Application.IsPlaying(gameObject)) return;
      if(string.IsNullOrEmpty(gameObject.scene.path)) return;

      SerializedObject serializedObject = new SerializedObject(this);
      SerializedProperty serializedProperty = serializedObject.FindProperty("_uniqueIdentifier");

      if (string.IsNullOrEmpty(serializedProperty.stringValue) || !IsUnique(serializedProperty.stringValue))
      {
         serializedProperty.stringValue = System.Guid.NewGuid().ToString();
         serializedObject.ApplyModifiedProperties();
      }

      globalLookThrough[serializedProperty.stringValue] = this;
   }

   private bool IsUnique(string candidate)
   { 
      print("Here it is");

      if(!globalLookThrough.ContainsKey(candidate))
      {
         return true;
      }

      if (globalLookThrough[candidate] == this)
      {
         return true;
      }

      if (globalLookThrough[candidate] == null)
      {
         globalLookThrough.Remove(candidate);
         return true;
      }

      if (globalLookThrough[candidate].GetUniqueIdentifier() != candidate)
      {
         globalLookThrough.Remove(candidate);
         return true;
      }
      
      return false;
   }
#endif
}
