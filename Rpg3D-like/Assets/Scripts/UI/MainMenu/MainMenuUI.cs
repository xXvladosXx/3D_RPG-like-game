using System;
using System.IO;
using System.Linq;
using SavingSystem;
using UnityEngine;

namespace UI.MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject button;
    
        private Lazy<SavingHandler> _savingHandler;
        private string _saveFile = " ";
        private string _lastSave;
        private void Awake()
        {
            _savingHandler = new Lazy<SavingHandler>(GetSavingHandler);
        
            _lastSave = Directory.GetFiles(Path.Combine(Application.persistentDataPath))
                .Select(x => new FileInfo(x))
                .OrderByDescending(x => x.LastWriteTime)
                .FirstOrDefault()
                ?.ToString();

            if (_lastSave == null)
            {
                button.SetActive(false);
            }
        }

        public void CreateName(string saveFile)
        {

            _saveFile = saveFile;

        }

        public void StartNewGame()
        {
            _savingHandler.Value.StartNewGame(_saveFile);
        }
        public void ContinueGame()
        {
            _savingHandler.Value.ContinueGame(_lastSave);
        }

        private SavingHandler GetSavingHandler()
        {
            return FindObjectOfType<SavingHandler>();
        }
    }
}
