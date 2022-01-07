using System.ComponentModel.DataAnnotations;

namespace InventoryModels
{
    public class Category : FullAuditModel
    {
        [StringLength(InventoryModelsConstants.MAX_NAME_LENGTH)]
        public string Name { get; set; }

        public List<Item> Items { get; set; } = new List<Item>();
    }
}
