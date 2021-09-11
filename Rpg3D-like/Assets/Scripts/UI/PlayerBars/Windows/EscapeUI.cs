using System;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace.Scenes
{
    [RequireComponent(typeof(Canvas))]
    public class EscapeUI : MonoBehaviour, IChangable
    {
        public KeyCode KeyCode;
        public bool HasUIToDisalbe;

        
        private CanvasController _canvasController;
        private void Awake()
        {
            _canvasController = FindObjectOfType<CanvasController>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode) && !CheckUIs())
            {
                if (_canvasController.CanShowNewUIs)
                {
                    Show();
                    PauseGame(Convert.ToSingle(0));
                }
                else
                {
                    Hide();
                    PauseGame(Convert.ToSingle(1));
                }
            }
            
            if (Input.GetKeyDown(KeyCode) && CheckUIs())
            {
                _canvasController.GetList.LastOrDefault()?.GetComponent<IChangable>().Hide();
            }
        }

        private bool CheckUIs()
        {
            return HasUIToDisalbe = _canvasController.GetList.Count > 0;
        }


        public event Action Opened;
        public event Action Closed;

        public bool IsActive { get; }

        public void Hide()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            _canvasController.CanShowNewUIs = true;
        }

        public void Show()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            
            _canvasController.CanShowNewUIs = false;
        }
        
        private void PauseGame(float isPaused)
        {
            Time.timeScale = isPaused;
        }
    }
}