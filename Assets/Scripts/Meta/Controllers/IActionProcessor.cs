using Meta.Configs;


namespace Meta.Controllers
{
    //Processor=>Handler?
    public interface IActionProcessor
    {
        void Process(IActionConfig actionConfig);
    }

    public abstract class ActionProcessorAbstract<TArgs> : IActionProcessor// where TArgs : IActionConfig
    {
        public void Process(IActionConfig actionConfig)
        {
            Process((TArgs) actionConfig);
        }
        protected abstract void Process(TArgs args);
    }
}