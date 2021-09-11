using System.Collections;
using CharacterSelection;
using UnityEngine;

namespace SceneManagement
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private Camera _characterCamera;
        [SerializeField] private Camera _dollyCamera;
        [SerializeField] private GameObject _timelime;
        [SerializeField] private GameObject _textSeelecting;
    
        private IEnumerator WaitToDisableCamera()
        {
            yield return new WaitForSeconds(5.2f);
            _dollyCamera.gameObject.SetActive(false);
            _characterCamera.GetComponent<CameraController>().enabled = true;
            _textSeelecting.SetActive(true);
        }

        public void StartCameraMovement()
        {
            StartCoroutine(WaitToDisableCamera());
            _timelime.SetActive(true);
        }
    }
}
