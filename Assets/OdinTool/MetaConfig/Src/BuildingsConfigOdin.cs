using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Meta.ConfigOdin
{
    public class BuildingsConfigOdin : ConfigElem
    { 
        //стоимость, время строительства, уровни, ресурсы, что производит, что требует
        //таски с рекрутами?
        //список строений, какие строения доступны игроку (требования?).
        
        // - увеличивает лимит ресурса
        // - прозволяет производить юнитов (доступ к возможности)
        // - позволяет апгрейдить юнитов (доступ к возможности)
        // - производит ресурс (список таск, кот можно запустить)
        
        
        //Подумать, как удобнее будет настраивать игру, таски и тд для ГД.
        //тип если таски принадлежат зданию - тут их и создавать и держать? Будет аналогично игре.
        
        //Здание - Барраки
        //Макс кол-во 
        //Кого и за сколько могут производить? (таски?) Макс количество юнитов ТИпа?
        //? Какие требования для каждого юнитв должны быть выполнены?
        
        //Здание - Шахта. 
        //макс коли-во
        //какие таски по добыче есть на выбор
        //типы тасок - автокомплит, автозапуск?
        
        //Здание - лаборатория 
        //макс кол-во
        //какие апгрейды может выполнять (и как? моментально, за прайс, за дабл прайс)
        //условия? 
        
        
        public BuildingUnits Units = new BuildingUnits();
    }
    
    [System.Serializable]
    public class BuildingUnits
    {
        //purchase?
        public List<ActionTrain> Units = new List<ActionTrain>();
    }
    
    public class Action{}


    [System.Serializable]
    public class ActionTrain : Action
    {
        [VerticalGroup("Costs/Left"), HideLabel] 
        [HorizontalGroup("Costs", Width = 200)]
        public UnitConfigOdin Unit;

        [VerticalGroup("Costs/Left")] 
        [HorizontalGroup("Costs", Width = 200)]
        public int Limit;

        [HorizontalGroup("Costs/Right"), HideLabel]
        public EntityCollections Costs = new EntityCollections();
    }




}