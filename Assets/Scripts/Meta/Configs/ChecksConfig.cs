using System.Collections.Generic;
using AppRen;

namespace Meta.Configs
{
    public interface ICheckVisitor
    {
        bool Visit(CheckUnitLimit unitLimit);
        bool Visit(IEnumerable<ICheckEntityConfig> collection);
    }
    
    public interface ICheckEntityConfig
    {
        bool CheckVisit(ICheckVisitor visitor);
    }
    
    public class CheckUnitLimit : ICheckEntityConfig
    {
        public Id TypeUnit;
        //condition?
        public int Count;
        
        public bool CheckVisit(ICheckVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

}