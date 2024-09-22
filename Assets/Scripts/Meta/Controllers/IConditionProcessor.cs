using Meta.Configs;

namespace Meta.Controllers
{
    public interface IConditionProcessor
    {
        bool Check(IConditionConfig conditionConfig);
        //getText?
        //getToast?
    }
    
    public abstract class ConditionProcessorAbstract<TArgs> : IConditionProcessor where TArgs : IConditionConfig, new()
    {
        public bool Check(IConditionConfig conditionConfig)
        {
            //кастниг всего один да еще и автоматический.
            return Check((TArgs) conditionConfig);
        }

        protected abstract bool Check(TArgs conditionsConfig);

        //если потребуется
        public static TArgs CreateEmptyArgs()
        {
            return new TArgs();
        }
    }

}