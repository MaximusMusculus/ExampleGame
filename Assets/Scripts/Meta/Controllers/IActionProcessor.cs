using Meta.Configs;


namespace Meta.Controllers
{
    public interface IActionProcessor
    {
        void Process(IActionConfig config);
    }

    public abstract class ActionAbstract<TArgs> : IActionProcessor where TArgs : IActionConfig, new()
    {
        public void Process(IActionConfig config)
        {
            Process((TArgs) config);
        }

        protected abstract void Process(TArgs args);

        
        public static TArgs CreateEmptyArgs()
        {
            return new TArgs();
        }
    }
}