using System;
using UnityEngine;

namespace DefaultNamespace.Scenes
{
    [RequireComponent(typeof(Canvas))]
    public class ChangeUI : MonoBehaviour, IChangable
    {
        public event Action Opened;
        public event Action Closed;
        public bool IsActive { get; private set; }

        protected CanvasController _canvasController;
        
        private void Awake()
        {
            _canvasController = FindObjectOfType<CanvasController>();
        }
        public void Hide()
        {
            if (!IsActive) return;
            _canvasController.RemoveUI(gameObject);

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            
            IsActive = false;
            Closed?.Invoke();
        }

        public void Show()
        {
            if(IsActive) return;

            _canvasController.AddUI(gameObject);

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            
            IsActive = true;
            Opened?.Invoke();
        }
    }
}