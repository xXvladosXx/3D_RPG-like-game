using DefaultNamespace.Scenes;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Tooltip : MonoBehaviour
    {
        private static Tooltip Instance;

        [SerializeField] private RectTransform _canvas;
        private RectTransform _background;
        private TextMeshProUGUI _text;
        private RectTransform _rectTransform;
        private ChangeUI _changeUI;
    
        private void Awake()
        {
            Instance = this;
            _canvas = GameObject.FindWithTag("MainCanvas").GetComponent<RectTransform>();
            _background = transform.Find("Background").GetComponent<RectTransform>();
            _text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
            _rectTransform = GetComponent<RectTransform>();
            _changeUI = GetComponent<ChangeUI>();
     
            HideTooltip();
        }

        private void SetText(string tooltipText)
        {
            _text.SetText(tooltipText);
            _text.ForceMeshUpdate();
            Vector2 textSize = _text.GetRenderedValues(false);
            var textMargin = _text.margin * 2;

            _background.sizeDelta = textSize + new Vector2(textMargin.x, textMargin.y);
            this.Update();
        }

        private void Update()
        {
            Vector2 anchoredPosition = (Input.mousePosition )/ _canvas.localScale.x;

            if (anchoredPosition.x + _background.rect.width > _canvas.rect.width)
            {
                anchoredPosition.x = _canvas.rect.width - _background.rect.width;
            }
        
            if (anchoredPosition.y + _background.rect.height > _canvas.rect.height)
            {
                anchoredPosition.y = _canvas.rect.height - _background.rect.height;
            }
        
            _rectTransform.anchoredPosition = anchoredPosition;
        }

        private void ShowTooltip(string tooltipText)
        {
            _changeUI.Show();
            gameObject.SetActive(true);
            SetText(tooltipText);
        }

        private void HideTooltip()
        {
            _changeUI.Hide();
            gameObject.SetActive(false);
        }

        public static void EnableTooltip(string tooltip)
        {
            Instance.ShowTooltip(tooltip);
        }

        public static void DisableTooltip()
        {
            Instance.HideTooltip();
        }
    }
}
