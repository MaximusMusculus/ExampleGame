using Meta.Configs.Actions;
using Meta.Configs.Conditions;

namespace Meta.Configs
{
    
    // Игровое действие, имеет проверки, меняет стейт?
    public class MetaActionConfig
    {
        public ConditionCollectionConfig Require;
        public ActionCollectionConfig Actions;
    }

    //как можно представить игровые действия?
    //как можно представить группу игровых действий?
    //как их группировать?
    //как с ними работать?

    //Вызов экшена пока статичен. Когда он станет динамичным - когда появятся самостоятельные сущности по типу эвентов и баттл пассов
    //тогда будет создан их контроллер и дто.
}