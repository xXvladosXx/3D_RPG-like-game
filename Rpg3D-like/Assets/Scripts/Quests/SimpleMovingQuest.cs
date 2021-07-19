using System;
using UnityEngine;

namespace Quests
{
    public class SimpleMovingQuest : IMoveable
    {
        public event Action OnUpdateQuest;
        public void MoveTo(GameObject player, GameObject other)
        {
            player.GetComponent<Movement>().MoveTo(other.transform.position, 1f);
        }
    }
}