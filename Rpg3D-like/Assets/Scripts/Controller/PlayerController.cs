using System;
using System.Collections;
using Controller.States;
using DialogueSystem.Interaction;
using Stats;
using UI.Cursor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _clickedEffect;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private ObjectPooler.ObjectPooler _objectPooler;
        [SerializeField] private ActionScheduler _actionScheduler;
        public event Action<Transform> OnEnemyAttacked;

        private Health _health;

        public enum CursorType
        {
            None,
            Attack,
            Move,
            UI,
            PickUp,
            Shop,
            Quest,
            Upgrade
        }

        [Serializable]
        struct CursorIterating
        {
            public CursorType Type;
            public Vector2 Hotspot;
            public Texture2D Texture;
        }

        [SerializeField] private CursorIterating[] _cursorIteratings;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        void Update()
        {
            if(_health == null) return;
            if(PointerOverUI()) return;

            if (_actionScheduler.GetCurrentAction is IUnchangeableState)
            {
                return;
            }
            
            if (_health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }

            if(InteractWithComponent()) return;
            if (Movement()) return;

            SetCursor(CursorType.None);
        }

        private bool PointerOverUI()
        {
            if (!EventSystem.current.IsPointerOverGameObject()) return false;
        
            SetCursor(CursorType.UI);
            return true;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetRay());

            foreach (var hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (var raycastable in raycastables)
                {
                    if (!raycastable.HandleRaycast(this)) continue;
                
                    if(FindObjectOfType<CombatTarget>() != null)
                        OnEnemyAttacked?.Invoke(FindObjectOfType<CombatTarget>().transform);
                    
                    SetCursor(raycastable.GetCursorType());
                    return true;
                }
            }
        
            return false;
        }

        private void SetCursor(CursorType type)
        {
            CursorIterating iterating = GetCursorIterating(type);
            Cursor.SetCursor(iterating.Texture, iterating.Hotspot, CursorMode.Auto);
        }

        private CursorIterating GetCursorIterating(CursorType type)
        {
            foreach (var cursorIterating in _cursorIteratings)
            {
                if (cursorIterating.Type == type)
                    return cursorIterating;
            }

            return _cursorIteratings[0];
        }

        private bool Movement()
        {
            RaycastHit raycastHit;
            bool hasHit = Physics.Raycast(GetRay(), out raycastHit);

            if (hasHit)
            {
                if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    GetComponent<Movement>().StartMoveToAction(raycastHit.point, 1f);
                
                    if (Physics.Raycast(GetRay(), out raycastHit, 1000, _layerMask))
                    {

                        if (_objectPooler.GetPooledObject() == null)
                        {
                            SetCursor(CursorType.Move);
                            return true;
                        }
                    
                        ParticleSystem clickedEffect = _objectPooler.GetPooledObject().GetComponent<ParticleSystem>();

                        clickedEffect.transform.position = new Vector3(raycastHit.point.x, raycastHit.point.y, raycastHit.point.z);
                        clickedEffect.transform.rotation = Quaternion.identity;

                        clickedEffect.gameObject.SetActive(true);
                        StartCoroutine(WaitToDisableClickedEffect(clickedEffect));
                    }
                }

                SetCursor(CursorType.Move);
                return true;
            }

            return false;
        }

        private IEnumerator WaitToDisableClickedEffect(ParticleSystem clickedEffect)
        {
            yield return new WaitForSeconds(0.5f);
            clickedEffect.gameObject.SetActive(false);
        }

        public static Ray GetRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}