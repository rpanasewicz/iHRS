using System.Threading.Tasks;

namespace iHRS.Application.Common
{
    public class Unit
    {
        public static Unit Value => new Unit();
        public static Task<Unit> Task => System.Threading.Tasks.Task.FromResult(Unit.Value);
    }
}
