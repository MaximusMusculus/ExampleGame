using Meta.Configs;

namespace Meta.Controllers
{
    public interface ICondition
    {
        bool Check(IConditionConfig conditionConfig);
        //getText?
        //getToast?
    }
    
    public abstract class ConditionAbstract<TArgs> : ICondition where TArgs : IConditionConfig, new()
    {
        public bool Check(IConditionConfig conditionConfig)
        {
            //кастниг всего один да еще и автоматический.
            return Check((TArgs) conditionConfig);
        }

        protected abstract bool Check(TArgs args);

        //если потребуется
        public static TArgs CreateEmptyArgs()
        {
            return new TArgs();
        }
    }

}