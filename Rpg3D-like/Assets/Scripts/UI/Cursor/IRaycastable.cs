using Controller;

namespace UI.Cursor
{
    public interface IRaycastable
    {
        PlayerController.CursorType GetCursorType();
        bool HandleRaycast(PlayerController player = null);
    }
}