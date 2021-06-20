 using System;
 using System.Collections;
using System.Collections.Generic;
 using Scriptable.Weapon;
 using UnityEngine;
 using UnityEngine.XR;

 [CreateAssetMenu(fileName = "DelayClickTargeting", menuName = "Abilities/DelayClick", order = 0)]
 public class DelayClickTargeting : TargetingStrategy
 {
     [SerializeField] private Texture2D _cursorTexturem;
     [SerializeField] private Vector2 _cursorHotspot;
     [SerializeField] private float _skillRadius;
     [SerializeField] private LayerMask _layerMask;
     [SerializeField] private Transform _skillRendererRadius;
     [SerializeField] private float _distanceToCastSkill;
     

     private Transform _skillRendererRadiusInstance;
     public override void StartTargeting(SkillData skillData, Action finishedAttack, Action cancelAttack)
     {
         PlayerController playerController = skillData.GetUser.GetComponent<PlayerController>();

         playerController.StartCoroutine(Targeting(skillData, playerController, finishedAttack, cancelAttack));
     }

     private IEnumerator Targeting(SkillData skillData, PlayerController playerController, Action finishedAttack, Action canceledAttack)
     {
         playerController.enabled = false;

         if (_skillRendererRadiusInstance == null)
         {
             _skillRendererRadiusInstance = Instantiate(_skillRendererRadius);
         }
         else
         {
             _skillRendererRadiusInstance.gameObject.SetActive(true);
         }

         _skillRendererRadiusInstance.localScale = new Vector3(_skillRadius*2, 1, _skillRadius*2);
         while (true)
         {
            
             RaycastHit raycastHit;

             Cursor.SetCursor(_cursorTexturem, _cursorHotspot, CursorMode.Auto);

             if (Physics.Raycast(PlayerController.GetRay(), out raycastHit, 1000, _layerMask))
             {
                 _skillRendererRadiusInstance.position = raycastHit.point;
                 
                 var newDistance = Vector3.Distance(skillData.GetUser.transform.position, raycastHit.point);
             
                 if (Input.GetMouseButton(0))
                 {
                     if (newDistance > _distanceToCastSkill)
                     {
                         Debug.Log("not");
                         yield return null;
                     }
                     else
                     {
                         while (Input.GetMouseButton(0))
                         {
                             yield return null;
                         }

                         skillData.SetMousePosition(raycastHit.point);

                         playerController.enabled = true;
                         Cursor.SetCursor(default, default, CursorMode.Auto);

                         skillData.SetTargets(GetGameObjectsInRadius(raycastHit.point));
                         finishedAttack();
                         _skillRendererRadiusInstance.gameObject.SetActive(false);

                         yield break;
                     }
                 }else if (Input.GetMouseButton(1))
                 {
                     Canceling(playerController);

                     canceledAttack();
                     yield break;
                 }
             }

             yield return null;
         }
     }

     private void Canceling(PlayerController playerController)
     {
         playerController.enabled = true;
         Cursor.SetCursor(default, default, CursorMode.Auto);
         _skillRendererRadiusInstance.gameObject.SetActive(false);
     }
     private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
     {
         RaycastHit[] hits = Physics.SphereCastAll(point, _skillRadius, Vector3.up, 0);
         foreach (var hit in hits)
         {
             yield return hit.collider.gameObject;
         }
     }
 }
