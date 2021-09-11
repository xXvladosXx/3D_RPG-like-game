using System.Collections;
using System.Collections.Generic;
using System.IO;
using SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SavingSystem
{
    public class SavingHandler : MonoBehaviour
    {
        private const string _defaultSaveFile = "QuickSave";

        public void StartNewGame(string saveFile)
        {
            StartCoroutine(LoadStartScene(saveFile));
        }

        public void ContinueGame(string saveFile = _defaultSaveFile)
        {
            saveFile = Path.GetFileNameWithoutExtension(saveFile);

            StartCoroutine(LoadScene(saveFile));
        }

        public void LoadGame(string saveFile = _defaultSaveFile)
        {
            StartCoroutine(LoadScene(saveFile));
        }

        private IEnumerator LoadScene(string saveFile)
        {
            LevelLoader.Instance.StartFading();

            DontDestroyOnLoad(gameObject);
            yield return GetComponent<Saving>().LoadScene(saveFile);
        }

        private IEnumerator LoadStartScene(string saveFile)
        {
            LevelLoader.Instance.StartFading();

            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(1);
            Save(saveFile);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save(_defaultSaveFile);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Load(_defaultSaveFile);
            }
        }

        public void Load(string saveFile)
        {
            GetComponent<Saving>().Load(saveFile);
        }

        public void Save(string saveFile)
        {
            GetComponent<Saving>().Save(saveFile);
        }

        public IEnumerable<string> SaveList()
        {
            return GetComponent<Saving>().SavesList();
        }
    }
}