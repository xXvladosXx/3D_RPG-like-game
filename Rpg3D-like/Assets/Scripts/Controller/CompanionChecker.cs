using UnityEngine;

namespace Controller
{
    public class CompanionChecker : MonoBehaviour
    {
        private FriendlyAIController _companion;
        
        public FriendlyAIController GetCompanion => _companion;
        
        public void SetCompanion(FriendlyAIController companion)
        {
            _companion = companion;
        }
        public bool HasCompanion()
        {
            return _companion != null;
        }

        public void DestroyCompanion()
        {
            Destroy(_companion.gameObject);
        }
    }
}