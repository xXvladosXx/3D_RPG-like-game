namespace StateMachine
{
    public interface IFriendlyAIStateSwitcher
    {
        void SwitchState<T>() where T : BaseState;
    }
}