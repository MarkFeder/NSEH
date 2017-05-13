using System.Collections;

namespace nseh.Gameplay.Base.Interfaces
{
    public interface IAction
    {
        int Hash { get; }

        bool IsEnabled { get; set; }

        void StartAction(float value);

        void StartAction(int value);

        void StartAction();

        void StopAction();

        bool ButtonHasBeenPressed();

        bool ButtonHasBeenReleased();

        bool ButtonIsHoldDown();

        bool ReceiveInput();
    }
}
