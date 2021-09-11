using System;
using Controller;
using Scriptable.Weapon;
using UI.Cursor;
using UnityEngine;

namespace ItemPickUp
{
    public class ChestPickUp : MonoBehaviour, IRaycastable
    {
        [SerializeField] private ChestScriptable _chest;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerController playerController))
            {
                playerController.GetComponent<Equipment>().Equip(_chest);

                foreach (Transform child in transform) {
                    Destroy(child.gameObject);
                }
                Destroy(gameObject);
            }
        }

        public PlayerController.CursorType GetCursorType()
        {
            return PlayerController.CursorType.PickUp;
        }

        public bool HandleRaycast(PlayerController player)
        {
            if (Input.GetMouseButton(0))
            {
                player.GetComponent<Movement>().StartMoveToAction(gameObject.transform.position, 1f);
            }
            return true;
        }
    }
}