using System.Collections;
using UnityEngine;

namespace SceneManagement
{
    public class LevelLoader : MonoBehaviour
    {
        public static LevelLoader Instance { get; set; }
        [SerializeField] private Animator _transition;
        [SerializeField] private float _transitionTime;
        private void Awake()
        {
            Instance = this;
        }

        public void StartFading()
        {
            StartCoroutine(LoadLevel());
        }

        private IEnumerator LoadLevel()
        {
            _transition.SetTrigger("Start");

            yield return new WaitForSeconds(_transitionTime);
        }
    }
}
