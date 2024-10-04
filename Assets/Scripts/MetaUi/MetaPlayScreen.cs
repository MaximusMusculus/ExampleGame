using Meta;
using UnityEngine;

namespace MetaUi
{
    public class MetaPlayScreen : MonoBehaviour
    {
        [SerializeField] private MetaItemsBar _itemsBar;
        [SerializeField] private MetaQuestWidget _metaQuestWidget;
        
        public void Setup(MetaModel metaModel, ISpriteHolderTest spriteHolderTest)
        {
            _itemsBar.Setup(metaModel.Inventory, spriteHolderTest);
            _metaQuestWidget.Setup(metaModel.Quests);
        }

        public void UpdateView()
        {
            _itemsBar.UpdateView();
            _metaQuestWidget.UpdateView();
        }
    }
}
