using Meta.Configs;


namespace Meta.Controllers
{
    public interface IActionProcessor
    {
        void Process(IActionConfig actionConfig);
    }

    public abstract class ActionProcessorAbstract<TArgs> : IActionProcessor
    {
        public void Process(IActionConfig actionConfig)
        {
            Process((TArgs) actionConfig);
        }
        protected abstract void Process(TArgs action);
    }
}