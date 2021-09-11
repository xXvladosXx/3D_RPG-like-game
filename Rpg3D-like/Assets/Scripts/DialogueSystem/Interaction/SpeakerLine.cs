using UnityEngine;

namespace DialogueSystem.Interaction
{
    [CreateAssetMenu(menuName = "Speaker/Line")]
    public class SpeakerLine : ScriptableObject
    {
        [SerializeField] private Speaker _speaker;
        [SerializeField] private string _text;

        public Speaker GetSpeaker => _speaker;
        public string GetText => _text;
    }
}