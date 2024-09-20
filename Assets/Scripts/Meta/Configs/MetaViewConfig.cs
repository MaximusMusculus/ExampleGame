using System.Collections.Generic;
using AppRen;

namespace Meta.Configs
{
    public class MetaViewConfig
    {
        public int ConfigVersion;

        // Поля для формирования меты можно хранить отдельно, но стоит ли выделять общие элементы?
        //использовать это будет только вьюха. 
        public List<LocalizedDescription> DescriptionConfigs = new List<LocalizedDescription>();
        public List<IconMapping> IconsConfigs = new List<IconMapping>();
        public List<DialogMapping> DialogConfigs = new List<DialogMapping>();
    }

    public class LocalizedDescription
    {
        public Id TargetId;
        public string nameKey;
        public string descriptionKey;
    }
    public class IconMapping
    {
        public Id TargetId;
        public string IconPath;
    }
    public class DialogMapping
    {
        public Id TargetId;
        public string DialogPath;
    }
    
}