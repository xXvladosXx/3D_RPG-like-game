using System;
using DefaultNamespace;
using UnityEngine;

namespace UI.PlayerBars.NewUI
{
    [RequireComponent(typeof(IChangable))]
    public class UIActivator : MonoBehaviour
    {
        [SerializeField] private KeyCode KeyCode;
        private IChangable _ui;
        private CanvasController _canvasController;

        private void Awake()
        {
            _canvasController = FindObjectOfType<CanvasController>();
            _ui = GetComponent<IChangable>();
        }


        private void Update()
        {
            if (!_canvasController.CanShowNewUIs) return;
            if (Input.GetKeyDown(KeyCode) && _ui.IsActive)
            {
                _ui.Hide();
                return;
            }

            if (Input.GetKeyDown(KeyCode) && !_ui.IsActive)
            {
                _ui.Show();
            }
        }
    }
}