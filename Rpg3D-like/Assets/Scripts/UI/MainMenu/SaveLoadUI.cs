using SavingSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class SaveLoadUI : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private GameObject _buttonPref;
        [SerializeField] private bool _saving = false;
    
        private SavingHandler _savingHandler;
        private void OnEnable()
        {
            _savingHandler = FindObjectOfType<SavingHandler>();
        
            if(_savingHandler == null) return;
        
            foreach (Transform child in _content)
            {
                Destroy(child.gameObject);
            }
            foreach (var save in _savingHandler.SaveList())
            {
                GameObject savePrefab = Instantiate(_buttonPref, _content);
                savePrefab.GetComponentInChildren<TextMeshProUGUI>().text = save;
            
                Button buttonSave = savePrefab.GetComponent<Button>();
                if (!_saving)
                {
                    buttonSave.onClick.AddListener((() => { _savingHandler.LoadGame(save); }));
                }
                else
                {
                    buttonSave.onClick.AddListener((() => { _savingHandler.Save(save); }));
                }
            }
        }

    }
}
