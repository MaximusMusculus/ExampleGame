using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;


namespace Meta.ConfigOdin
{
    public interface IDropDownElemProvider
    {
        IEnumerable GetDropDownElems();
    }
    public class MetaDropDownElemProvider : IDropDownElemProvider
    {
        private readonly IHashCode _idCounter;
        private readonly MetaConfigOdin _config;
        private readonly ValueDropdownList<ulong> _dropDownElems;
        
        
        //Одинаковый хешкод - не гарантия одинаковых объектов.
        private int _lastHast;
        
        public MetaDropDownElemProvider(IHashCode idCounter, MetaConfigOdin config)
        {
            _idCounter = idCounter;
            _config = config;
            _dropDownElems = new ValueDropdownList<ulong>();
        }

        //public ValueDropdownList<ulong> GetDropDown() //Filter
        
            
        
        
        private void CreateDropDownElem()
        {
            _dropDownElems.Clear();
            _lastHast = _idCounter.GetHashCode();
            
            _dropDownElems.AddRange(_config.Resources.Select(resource => new ValueDropdownItem<ulong>(resource.TechName, resource.ResourceType)));
            _dropDownElems.AddRange(_config.Units.Select(unit => new ValueDropdownItem<ulong>(unit.TechName, unit.UnitType)));
            _dropDownElems.AddRange(_config.Quests.Select(quest => new ValueDropdownItem<ulong>(quest.TechName, quest.QuestType)));

            /*{ "Node 1/Node 1.1", 1 },
            { "Node 1/Node 1.2", 2 },
            { "Node 2/Node 2.1", 3 },
            { "Node 3/Node 3.1", 4 },
            { "Node 3/Node 3.2", 5 },
            { "Node 1/Node 3.1/Node 3.1.1", 6 },
            { "Node 1/Node 3.1/Node 3.1.2", 7 },*/
        }
        

        public IEnumerable GetDropDownElems()
        {
            if (_lastHast != _idCounter.GetHashCode())
            {
                CreateDropDownElem();
            }
            return _dropDownElems;
        }
    }
    
}