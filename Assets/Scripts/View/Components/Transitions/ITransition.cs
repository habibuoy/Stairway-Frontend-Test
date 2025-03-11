using System.Threading.Tasks;

namespace Game.UI.View.Components.Transitions
{
    public interface ITransition
    {
        void Initialize();

        Task In();
        
        Task Out();
    }
}