using System.Collections.Generic;
using AppRen;
using Meta.Configs;
using Meta.Models;

namespace Meta.Controllers
{
    public interface IExchangesController
    {
        Id Add(ExchangeConfig config);
        void Remove(Id item);
        void Exchange(Id item);
        
        IEnumerable<ExchangeModel> GetExchanges();  
    }
    

    /// <summary>
    /// Описание того, как работают покупки.
    /// Наем юнитов, покупка юнитов, покупка чего либо.
    /// Все сторонние объекты, кот хотят, могут добавлять свои кусочки в список покупок.
    /// </summary>
    public class ExchangeModel
    {
        public Id ItemId { get; }
        
        public ExchangeModel(Id itemId)
        {
            ItemId = itemId;
        }
    }

    public interface IExchangeFactory
    {
        
    }



    /// <summary>
    /// Все покупки тут. Наем юнитов - тоже покупка.
    /// </summary>
    public class ExchangesController : IExchangesController
    {
        private List<ExchangeItemDto> _exchanges;
        private List<ExchangeConfig> _exchangesConfigs;
        //idProvider
        
        public ExchangesController(List<ExchangeConfig> exchangesConfigs, List<ExchangeItemDto> exchanges)
        {
            _exchanges = exchanges;
            _exchangesConfigs = exchangesConfigs;
        }

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
        
        public void Exchange(Id item)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ExchangeModel> GetExchanges()
        {
            throw new System.NotImplementedException();
        }
    }
}