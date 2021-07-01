using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AI;

[ExecuteAlways]
public class SaveableEntity : MonoBehaviour
{
   [SerializeField] private string _uniqueIdentifier = System.Guid.NewGuid().ToString();
   public string GetUniqueIdentifier()
   {
      return _uniqueIdentifier;
   }

   public object CaptureState()
   {
      return new SerializableVector(transform.position);
   }
   
   public void RestoreState(object state)
   {
      SerializableVector position =(SerializableVector)state;
      
      GetComponent<NavMeshAgent>().enabled = false;
      GetComponent<ActionScheduler>().Cancel();
      GetComponent<NavMeshAgent>().enabled = true;

      transform.position = position.ToVector();
   }

#if UNITY_EDITOR
   private void Update()
   {
      if(Application.IsPlaying(gameObject)) return;
      if(string.IsNullOrEmpty(gameObject.scene.path)) return;

      SerializedObject serializedObject = new SerializedObject(this);
      SerializedProperty serializedProperty = serializedObject.FindProperty("_uniqueIdentifier");

      if (string.IsNullOrEmpty(serializedProperty.stringValue))
      {
         serializedProperty.stringValue = System.Guid.NewGuid().ToString();
         serializedObject.ApplyModifiedProperties();
      }
   }
#endif
}
