

namespace Meta.ConfigOdin
{
    /// <summary>
    /// Всевозможные миссии, что встречаются в игре
    /// Содержит в себе все данные, необходимые для миссии.
    /// стоимость для старта, содержание внутри, награды за прохождение.
    /// тип миссии: разведка, высадка, бой. Возможны вложенные миссии (линейно одна за другой)
    /// 
    /// </summary>
    public class MissionsConfigOdin : ConfigElem
    {
        //id, сложность?
        //возможно иконки с описанием
        //type
        //cost to start
        //reward
        //....
        //...
        //параметры для задания мини игры 
    }

    public class ScoutingMission
    {
        //требования, что нужно, что бы было
        public EntityCollections Requirements = new EntityCollections();
        
        //что будет забрано
        public EntityCollections Costs = new EntityCollections();

        //что выдадут
        public EntityCollections Rewards = new EntityCollections();
    }

    public class MissionBattle
    {
        //требования, что нужно, что бы было
        public EntityCollections Requirements = new EntityCollections();
        
        //что будет забрано
        public EntityCollections Costs = new EntityCollections();

        //что выдадут
        public EntityCollections Rewards = new EntityCollections();
    }

    public class MissionsSequence
    {
        //missions?
    }

    // Миссия пвп, разведка, пве(сценарий), пве (эвент), 
    //в зависимости от миссий выдается награда и списывается ресурс.
    //если миссия боевая - списывается войско, кот ходило в миссиию.
    //повторяемые/одноразовые/событийные
    
    
    
}