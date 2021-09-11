using UnityEngine;

namespace UI.MainMenu
{
    public class MainMenuSwitcherUI : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenu;

        private void Start()
        {
            if(_mainMenu != null)
                SwitchTo(_mainMenu);
        }

        public void UIDisabler()
        {
            gameObject.transform.parent.gameObject.SetActive(false);
        }
        public void OnDisable()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        public void SwitchTo(GameObject switchTo)
        {
            if (switchTo.transform.parent != transform) return;

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(child.gameObject == switchTo);
            }
        }
    }
}
