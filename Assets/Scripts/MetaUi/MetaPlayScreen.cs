using Meta;
using UnityEngine;

namespace MetaUi
{
    public class MetaPlayScreen : MonoBehaviour
    {
        [SerializeField] private MetaItemsBar _itemsBar;

        public void Setup(MetaModel metaModel, ISpriteHolderTest spriteHolderTest)
        {
            _itemsBar.Setup(metaModel.Inventory, spriteHolderTest);
        }


        public void UpdateView()
        {
            _itemsBar.UpdateItems();
        }
    }
}
