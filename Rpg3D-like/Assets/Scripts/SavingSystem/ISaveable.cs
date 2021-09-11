

    namespace SavingSystem
    {
        public interface ISaveable
        {
            object CaptureState();
            void RestoreState(object state);
        }
    }
