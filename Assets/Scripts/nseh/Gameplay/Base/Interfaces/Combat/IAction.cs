namespace nseh.Gameplay.Base.Interfaces
{
    public interface IAction
    {
        int HashAnimation { get; set; }

        void DoAction(float value);

        void DoAction(int value);

        void DoAction();

        bool KeyHasBeenPressed();

        bool KeyHasBeenReleased();

        bool KeyIsHoldDown();

        bool ButtonHasBeenPressed();

        bool ButtonHasBeenReleased();

        bool ButtonIsHoldDown();

        bool ReceiveInput();
    }
}
