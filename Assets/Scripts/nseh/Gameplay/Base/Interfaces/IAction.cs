using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nseh.Gameplay.Base.Interfaces
{
    public interface IAction
    {
        bool KeyHasBeenPressed();

        bool ButtonHasBeenPressed();

        void DoAction(float value);

        void DoAction(int value);

        void DoAction();
    }
}
