using Controller;
using Inventory;

namespace Scriptable.UsableItems
{
    public abstract class HotBarItem : ItemObject
    {
        public abstract void UseItem(PlayerController playerController);
    }
}