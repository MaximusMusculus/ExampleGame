using Meta.Configs;


namespace Meta.Controllers
{
    //Processor=>Handler?
    public interface IActionProcessor
    {
        void Process(IActionConfig actionConfig);
    }

    public abstract class ActionAbstract<TArgs> : IActionProcessor where TArgs : IActionConfig, new()
    {
        public void Process(IActionConfig actionConfig)
        {
            Process((TArgs) actionConfig);
        }

        protected abstract void Process(TArgs args);

        
        public static TArgs CreateEmptyArgs()
        {
            return new TArgs();
        }
    }
}