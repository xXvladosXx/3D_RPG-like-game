using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Scriptable.Weapon.SkillsSpecification.Strategies;
using UnityEngine;

namespace Scriptable.Weapon.SkillsSpecification.Targeting
{
    [CreateAssetMenu(fileName = "DelayClickTargetingSquare", menuName = "Abilities/Targeting/DelayClickSquare",
        order = 0)]
    public class DelayClickTargetingSquare : TargetingStrategy, IAction
    {
        [SerializeField] private float _radius;
        [SerializeField] private Texture2D _cursorTexturem;
        [SerializeField] private Vector2 _cursorHotspot;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Vector3 _squareSize;
        [SerializeField] private Transform _skillCastDistance;
        [SerializeField] private int _heightSkillRenderer;
        [SerializeField] private float _rotationSpeed;

        private PlayerController _playerController;
        private DistanceRadiusRenderer _skillCastDistanceInstance;

        public override void StartTargeting(SkillData skillData, Action finishedAttack, Action cancelAttack)
        {
            _playerController = skillData.GetUser.GetComponent<PlayerController>();

            _playerController.StartCoroutine(Targeting(skillData, _playerController, finishedAttack, cancelAttack));
        }

        private IEnumerator Targeting(SkillData skillData, PlayerController playerController, Action finishedAttack,
            Action canceledAttack)
        {
            playerController.enabled = false;
            var rotation = skillData.GetUser.transform.rotation;

            _skillCastDistanceInstance = skillData.GetUser.GetComponentInChildren<DistanceRadiusRenderer>();
            
            _skillCastDistanceInstance.Manage(true);
            var skillTransform = _skillCastDistanceInstance.transform;
            skillTransform.position = skillData.GetUser.transform.position;

            skillTransform.localScale = new Vector3(_squareSize.x, _heightSkillRenderer, _squareSize.z);


            while (true)
            {
                RaycastHit raycastHit;
                var ray = PlayerController.GetRay();

                if (Physics.Raycast(ray, out raycastHit, 1000, _layerMask))
                {
                    var skillArea = skillTransform.position;

                    var direction = (raycastHit.point - skillData.GetUser.transform.position).normalized;

                    rotation = Quaternion.LookRotation(direction);
                    Vector3 euler = rotation.eulerAngles;
                    rotation = Quaternion.Euler(0, euler.y, 0);

                    skillTransform.rotation = Quaternion.Slerp(skillTransform.rotation,
                        rotation, Time.deltaTime * _rotationSpeed);

                    var distanceToPointX = Mathf.Abs(raycastHit.point.x - skillArea.x);
                    var distanceToPointZ = Mathf.Abs(raycastHit.point.z - skillArea.z);


                    if (Input.GetMouseButton(0))
                    {
                        if (!(_squareSize.x / 2 > distanceToPointX && _squareSize.z / 2 > distanceToPointZ))
                        {
                            while (Input.GetMouseButton(0))
                            {
                                yield return null;
                            }

                            skillData.SetMousePosition(skillData.GetUser.transform.position);

                            Cancel();

                            skillData.SetTargets(GetGameObjectsInSquare(skillData.GetUser.transform.position));
                            skillData.GetUser.transform.LookAt(raycastHit.point);

                            break;
                        }
                        else
                        {
                            while (Input.GetMouseButton(0))
                            {
                                yield return null;
                            }


                            skillData.SetMousePosition(raycastHit.point);

                            Cancel();

                            skillData.SetTargets(GetGameObjectsInSquare(raycastHit.point));
                            skillData.GetUser.transform.LookAt(raycastHit.point);

                            break;
                        }
                    }
                    else if (Input.GetMouseButton(1))
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

        private IEnumerable<GameObject> GetGameObjectsInSquare(Vector3 point)
        {
            RaycastHit[] hits = Physics.BoxCastAll(point,
                new Vector3(_squareSize.x / 2, _squareSize.y, _squareSize.z / 2), Vector3.up);
            foreach (var hit in hits)
            {
                yield return hit.collider.gameObject;
            }
        }

        public void Cancel()
        {
            _playerController.enabled = true;
            Cursor.SetCursor(default, default, CursorMode.Auto);
            _skillCastDistanceInstance.Manage(false);
        }
    }
}