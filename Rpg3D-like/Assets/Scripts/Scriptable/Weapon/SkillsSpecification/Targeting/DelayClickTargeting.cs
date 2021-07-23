 using System;
 using System.Collections;
using System.Collections.Generic;
 using Scriptable.Weapon;
 using UnityEngine;
 using UnityEngine.XR;

 [CreateAssetMenu(fileName = "DelayClickTargeting", menuName = "Abilities/DelayClick", order = 0)]
 public class DelayClickTargeting : TargetingStrategy, IAction
 {
     [SerializeField] private Texture2D _cursorTexturem;
     [SerializeField] private Vector2 _cursorHotspot;
     [SerializeField] private float _skillRadius;
     [SerializeField] private LayerMask _layerMask;
     [SerializeField] private Transform _skillRendererRadius;
     [SerializeField] private Transform _skillCastDistance;
     [SerializeField] private float _distanceToCastSkill;

     private PlayerController _playerController;
     private Transform _skillRendererRadiusInstance;
     private Transform _skillCastDistanceInstance;

     public override void StartTargeting(SkillData skillData, Action finishedAttack, Action cancelAttack)
     {
         _playerController = skillData.GetUser.GetComponent<PlayerController>();

         _playerController.StartCoroutine(Targeting(skillData, _playerController, finishedAttack, cancelAttack));
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
         
         if (_skillCastDistanceInstance == null)
         {
             _skillCastDistanceInstance = Instantiate(_skillCastDistance);
         }
         else
         {
             _skillCastDistanceInstance.gameObject.SetActive(true);
         }
         
         

         _skillRendererRadiusInstance.localScale = new Vector3(_skillRadius*2, 1, _skillRadius*2);
         _skillCastDistanceInstance.localScale = new Vector3(_distanceToCastSkill, 1,  _distanceToCastSkill);
         while (true)
         {
            
             RaycastHit raycastHit;

             Cursor.SetCursor(_cursorTexturem, _cursorHotspot, CursorMode.Auto);

             if (Physics.Raycast(PlayerController.GetRay(), out raycastHit, 1000, _layerMask))
             {
                 _skillRendererRadiusInstance.position = raycastHit.point;
                 _skillCastDistanceInstance.position = _playerController.gameObject.transform.position;
                 
                 var newDistance = Vector3.Distance(skillData.GetUser.transform.position, raycastHit.point);
             
                 if (Input.GetMouseButton(0))
                 {
                     if (newDistance > _distanceToCastSkill)
                     {
                         yield return null;
                     }
                     else
                     {
                         while (Input.GetMouseButton(0))
                         {
                             yield return null;
                         }

                         skillData.SetMousePosition(raycastHit.point);

                         Cancel();

                         skillData.SetTargets(GetGameObjectsInRadius(raycastHit.point));

                         break;
                     }
                 }else if (Input.GetMouseButton(1))
                 {
                     Cancel();
                     canceledAttack();      

                     yield break;
                 }
             }
             yield return null;
         }
         
         Cancel();
         finishedAttack();
     }

     private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
     {
         RaycastHit[] hits = Physics.SphereCastAll(point, _skillRadius, Vector3.up, 0);
         foreach (var hit in hits)
         {
             yield return hit.collider.gameObject;
         }
     }

     public void Cancel()
     {
         _playerController.enabled = true;
         Cursor.SetCursor(default, default, CursorMode.Auto);
         _skillRendererRadiusInstance.gameObject.SetActive(false);
         _skillCastDistanceInstance.gameObject.SetActive(false);
     }
 }
