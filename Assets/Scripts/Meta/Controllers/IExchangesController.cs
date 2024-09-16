using System;
using System.Collections.Generic;
using System.Linq;
using AppRen;
using Meta.Configs;
using Meta.Models;

namespace Meta.Controllers
{
    public interface IExchangesControllers
    {
        Id Add(ExchangeConfig config);
        void Remove(Id item);
        void DoExchange(Id item);
        
        IEnumerable<ExchangeController> GetExchanges();  
    }

    public interface ICheckUnitVisitor
    {
        bool Visit(CheckUnitLimit collection);
    }

    public class CheckUnitVisitor : ICheckUnitVisitor
    {
        private IUnitsController _unitsController;
        public bool Visit(CheckUnitLimit collection)
        {
            //проверяем, что у нас юнитов не (больще, меньше, равно) чем в конфиге.
            var targetUnit = _unitsController.GetUnits().FirstOrDefault(s => s.UnitType == collection.TypeUnit);
            return targetUnit.Count < collection.Count;
        }
    }

    public interface ICheckCollection
    {
        
    }

    public interface ICheckEntity
    {
        bool Visit(ICheckUnitVisitor unitVisitor);
    }

    public class CheckUnitLimit : ICheckEntity
    {
        public Id TypeUnit;
        //condition?
        public int Count; 
        
        public bool Visit(ICheckUnitVisitor unitVisitor)
        {
            return unitVisitor.Visit(this);
        }
    }
    
    

    /// <summary>
    /// Описание того, как работают покупки.
    /// Наем юнитов, покупка юнитов, покупка чего либо.
    /// Все сторонние объекты, кот хотят, могут добавлять свои кусочки в список покупок.
    /// </summary>
    public class ExchangeController
    {
        public Id ItemId { get; }
        public ExchangeConfig Config { get;}
        public ExchangeController(Id itemId)
        {
            ItemId = itemId;
        }
        
        //+DoExchange?  Spend, Add, Requirements
        //+task?<- формирует и запускает таск, который черех Х времени сделает У действие.
        //таск содержит только ту часть, в которой добавляется ресурс.

        private IVisitor _spendVisitor; //SpendVisitor 
        private IVisitor _addVisitor; //AddVisitor

        private CheckCanSpend _checkCanSpend;

        public bool Check()
        {
            //Константные 
            using (_checkCanSpend)
            {
                _checkCanSpend.Visit(Config.Spend);
                return _checkCanSpend.Result;
            }
        }

        public void DoExchange()
        {
            //способ выполнить набор действий не создаваю мелких сущностей и не прибегая к кастингу.
            _spendVisitor.Visit(Config.Spend);
            //если это таска - то создаем таску с передаче ей конфига.
            //таска сделает, то, что мы попросили через Х времени.
            _addVisitor.Visit(Config.Add);
        }
        
        
        public class CheckCanSpend : IVisitor, IDisposable
        {
            private IInventoryController _inventoryController;
            private IUnitsController _unitsController;
            
            public bool Result { get; private set; }
            private void Reset()
            {
                Result = false;
            }
            
            public void Dispose()
            {
                Reset();
            }
            
            public void Visit(EntityItem entityItem)
            {
                Result = _inventoryController.GetCount(entityItem.TypeItem) >= entityItem.Count;
            }
            
            public void Visit(EntityUnit entityItem)
            {
                var unit = _unitsController.GetUnits().FirstOrDefault(s=>s.UnitType == entityItem.TypeUnit);
                if (unit == null)
                {
                    Result = false;
                    return;
                }
                Result = unit.Count >= entityItem.Count;
            }

            public void Visit(IChangesCollection collection)
            {
                Result = true; 
                foreach (var entity in collection.Get())
                {
                    entity.Visit(this);
                    if (!Result)
                    {
                        break; // Выходим из цикла, если Result стал false
                    }
                }
            }
            
        }

    }
    
    
    

    public class ExchangeTaskController
    {
        //добавляет в req проверку на таск, если таск запущен - то не запускаем обмен.
        //при выполнении - делает кост. 
        //и создает такск на выполнение Адд. 
        //т.е. таска знает AddVisitor, время и id предмета.
    }

    public interface IExchangeFactory
    {
        
    }



    /// <summary>
    /// Все покупки тут. Наем юнитов - тоже покупка.
    /// </summary>
    public class ExchangesControllers : IExchangesControllers
    {
        private List<ExchangeItemDto> _exchanges;
        private List<ExchangeConfig> _exchangesConfigs;
        //idProvider
        
        public ExchangesControllers(List<ExchangeConfig> exchangesConfigs, List<ExchangeItemDto> exchanges)
        {
            _exchanges = exchanges;
            _exchangesConfigs = exchangesConfigs;
        }

        //а может быть несколько одинаковых эксченджей? Наверное - нет. В этом нет смысла.
        public Id Add(ExchangeConfig config)
        {
            //добавить айтем в пул
            throw new System.NotImplementedException();
        }
        
        public void Remove(Id item)
        {
            //удалить шопайтем из пула
            throw new System.NotImplementedException();
        }
        
        //Ui->CmdPurchase(Id,count);
        //CmdPurchase(Id,count)->PurchaseItem(Id);
        //PurchaseItem(Id)->CheckRequirements;/??
        //PurchaseItem(Id)->SpendChanges/AddChanges;
        
        public void DoExchange(Id item)
        {
            throw new System.NotImplementedException();
            //находим предмет. Юзаем его.
        }

        public IEnumerable<ExchangeController> GetExchanges()
        {
            throw new System.NotImplementedException();
        }
    }
}