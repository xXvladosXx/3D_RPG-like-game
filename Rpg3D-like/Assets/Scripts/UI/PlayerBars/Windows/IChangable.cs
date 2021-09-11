using System;

namespace DefaultNamespace
{
    public interface IChangable
    {
        public event Action Opened;
        public event Action Closed;
        public bool IsActive { get; }
        public void Hide();
        public void Show();
    }
}