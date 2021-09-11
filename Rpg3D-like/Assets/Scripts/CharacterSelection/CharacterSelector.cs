using System;
using System.Collections;
using Controller;
using Scriptable.Stats;
using UI.Cursor;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterSelection
{
    public class CharacterSelector : MonoBehaviour
    {
        [SerializeField] private Transform[] _characters;
        [SerializeField] private CameraController _camera;
        [SerializeField] private Transform _textSelection;
        [SerializeField] private Button _selectButton;
        [SerializeField] private CharacterInfo _characterInfo;
        [SerializeField] private Canvas _canvas;

        private CharacterInfo _currentCharacterInfo;
        private bool _canPresent = true;

        [Serializable]
        struct CursorIterating
        {
            public PlayerController.CursorType Type;
            public Vector2 Hotspot;
            public Texture2D Texture;
        }

        [SerializeField] private CursorIterating[] _cursorIteratings;

        private void Update()
        {
            if(_currentCharacterInfo!=null)
                print(_currentCharacterInfo.GetCharacterStat);
        
            if (Input.GetMouseButtonDown(1))
            {
                _camera.SetCharacterView(_characters[0]);
                if(_currentCharacterInfo!=null)
                    Destroy(_currentCharacterInfo.gameObject);

                if (!_textSelection.gameObject.activeSelf)
                {
                    _textSelection.gameObject.SetActive(true);
                    _selectButton.gameObject.SetActive(false);
                }
            }

            if (InteractWithCharacter()) return;
            SetCursor(PlayerController.CursorType.Move);
        }

        private bool InteractWithCharacter()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitToFindCharacter;
            GameObject character = null;

            if (Physics.Raycast(ray, out hitToFindCharacter))
            {
                var animator = hitToFindCharacter.transform.gameObject.GetComponent<Animator>();

                if (Input.GetMouseButtonDown(0) && animator!=null)
                {
                    character = hitToFindCharacter.transform.gameObject;

                    if (character == hitToFindCharacter.transform.gameObject)
                    {
                        _canPresent = true;
                        var view = character.transform.parent.GetComponentInChildren<ClickToInteract>();
                    
                        _textSelection.gameObject.SetActive(false);
                        _selectButton.gameObject.SetActive(true);
                        
                        if (_currentCharacterInfo == null)
                        {
                            _currentCharacterInfo = Instantiate(_characterInfo, _canvas.transform);
                            _currentCharacterInfo.SetCharacterInfo(character.GetComponent<FindStat>());
                        }

                        _camera.SetCharacterView(view.transform);
                    }
                    if (_canPresent)
                    {
                        animator.SetBool("isPresenting", true);
                        StartCoroutine(WaitEndOfAnimation(animator));
                        _canPresent = false;
                    }
                }
            }

        
            if (FindCursorType()) return true;

            return false;
        }

        private bool FindCursorType()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetRay());
            foreach (var hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (var raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast())
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        private IEnumerator WaitEndOfAnimation(Animator animator)
        {
            yield return new WaitForSeconds(1.5f);
            _canPresent = true;
            animator.SetBool("isPresenting", false);
        }

        private void SetCursor(PlayerController.CursorType type)
        {
            CursorIterating iterating = GetCursorIterating(type);
            Cursor.SetCursor(iterating.Texture, iterating.Hotspot, CursorMode.Auto);
        }

        private CursorIterating GetCursorIterating(PlayerController.CursorType type)
        {
            foreach (var cursorIterating in _cursorIteratings)
            {
                if (cursorIterating.Type == type)
                    return cursorIterating;
            }

            return _cursorIteratings[0];
        }

        private Ray GetRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        public void SetPlayer()
        {
        
        }
    }
}