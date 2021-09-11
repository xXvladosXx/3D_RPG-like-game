using UnityEngine;

namespace UI.Cursor
{
    public class DestroyEffect : MonoBehaviour
    {
        void Start()
        {
            Destroy(gameObject, 0.3f);
        }
    }
}
