namespace FridgeLogic.Patterns.FSM
{
    public interface IState
    {
        void OnUpdate(float dt);
        void OnEnter();
        void OnExit();
    }
}