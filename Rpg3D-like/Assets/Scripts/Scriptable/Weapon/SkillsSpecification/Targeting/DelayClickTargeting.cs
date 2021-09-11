using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Scriptable.Weapon.SkillsSpecification.Strategies;
using UnityEngine;

namespace Scriptable.Weapon.SkillsSpecification.Targeting
{
    [CreateAssetMenu(fileName = "DelayClickTargeting", menuName = "Abilities/Targeting/DelayClick", order = 0)]
    public class DelayClickTargeting : TargetingStrategy, IAction
    {
        [SerializeField] private Texture2D _cursorTexturem;
        [SerializeField] private Vector2 _cursorHotspot;
        [SerializeField] private float _skillRadius;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _distanceToCastSkill;

        private PlayerController _playerController;
        private SkillRadiusRadiusRenderer _skillRadiusRendererRadiusRadiusInstance;
        private DistanceRadiusRenderer _skillCastDistanceInstance;

        public override void StartTargeting(SkillData skillData, Action finishedAttack, Action cancelAttack)
        {
            _playerController = skillData.GetUser.GetComponent<PlayerController>();

            _playerController.StartCoroutine(Targeting(skillData, _playerController, finishedAttack, cancelAttack));
        }

        private IEnumerator Targeting(SkillData skillData, PlayerController playerController, Action finishedAttack, Action canceledAttack)
        {
            playerController.enabled = false;

            _skillRadiusRendererRadiusRadiusInstance = skillData.GetUser.GetComponentInChildren<SkillRadiusRadiusRenderer>();
            _skillCastDistanceInstance = skillData.GetUser.GetComponentInChildren<DistanceRadiusRenderer>();
            
            _skillCastDistanceInstance.Manage(true);
            _skillRadiusRendererRadiusRadiusInstance.Manage(true);
            
            _skillRadiusRendererRadiusRadiusInstance.transform.localScale = new Vector3(_skillRadius*2, 10, _skillRadius*2);
            _skillCastDistanceInstance.transform.localScale = new Vector3(_distanceToCastSkill, 10,  _distanceToCastSkill);
            
            while (true)
            {
            
                RaycastHit raycastHit;

                Cursor.SetCursor(_cursorTexturem, _cursorHotspot, CursorMode.Auto);

                if (Physics.Raycast(PlayerController.GetRay(), out raycastHit, 1000, _layerMask))
                {
                    _skillRadiusRendererRadiusRadiusInstance.transform.position = raycastHit.point;
                    _skillCastDistanceInstance.transform.position = _playerController.gameObject.transform.position;
                 
                    var newDistance = Vector3.Distance(skillData.GetUser.transform.position, raycastHit.point);
             
                    if (Input.GetMouseButton(0))
                    {
                        if (newDistance > _distanceToCastSkill/2)
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
                            skillData.GetUser.transform.LookAt(raycastHit.point);

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
            _skillRadiusRendererRadiusRadiusInstance.Manage(false);
            _skillCastDistanceInstance.Manage(false);
        }
    }
}