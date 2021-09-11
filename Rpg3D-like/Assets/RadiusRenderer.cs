using UnityEngine;

namespace DefaultNamespace
{
    public abstract class RadiusRenderer : MonoBehaviour
    {
        private MeshRenderer _mesh;

        private void Awake()
        {
            _mesh = GetComponent<MeshRenderer>();
        }

        public void Manage(bool enable)
        {
            _mesh.enabled = enable;
        }
    }
}