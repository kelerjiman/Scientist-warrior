namespace Script.CharacterStatus
{
    public interface IStateUI
    {
        void RemoveState(State state);
        void AddState(State state);
    }
}