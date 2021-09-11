using Controller;
using UnityEngine;

namespace Objects.Bosses
{
    public class EmptyCombat : MonoBehaviour
    {
        private PlayerController _playerController;

        private void Awake()
        {
            _playerController = FindObjectOfType<PlayerController>();
        }

        void Hit()
        {
        
        }
    
        void Shoot()
        {
            transform.LookAt(_playerController.transform);
        }
        void FootR()
        {
        
        }
    
        void FootL()
        {
        
        }
    }
}
