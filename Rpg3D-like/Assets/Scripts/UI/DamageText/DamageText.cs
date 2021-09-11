using TMPro;
using UnityEngine;

namespace UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        private TextMeshPro _textMeshPro;

        private void Awake()
        {
            _textMeshPro = GetComponent<TextMeshPro>();
        }

        public void SetDamageText(float amount)
        {
            transform.LookAt(Camera.main.transform);
            Vector3 euler = transform.rotation.eulerAngles;
            euler.y += 180;
        
            transform.rotation = Quaternion.Euler(euler);
            _textMeshPro.text = amount.ToString();
        }
    }
}
