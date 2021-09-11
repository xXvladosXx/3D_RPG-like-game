using UnityEngine;

namespace DialogueSystem.Interaction
{
    [CreateAssetMenu(menuName = "Speaker/Character")]
    public class Speaker : ScriptableObject
    {
        [SerializeField] private string _characterName;
        public string CharacterName => _characterName;
    }
}