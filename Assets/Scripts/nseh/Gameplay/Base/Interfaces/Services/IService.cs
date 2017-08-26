using nseh.Managers.Main;

namespace nseh.Gameplay.Base.Interfaces.Services
{
    public interface IService
    {
        bool IsActivated { get; set; }

        void Setup(GameManager manager);

        void Activate();

        void Tick();

        void Release();
    }
}
