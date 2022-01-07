using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryModels
{
    public class Item : FullAuditModel
    {
       
        [StringLength(InventoryModelsConstants.MAX_NAME_LENGTH)]
        [Required]
        public string Name { get; set; }

        [Range(InventoryModelsConstants.MINIMUM_QUANTITY, InventoryModelsConstants.MAXIMUM_QUANTITY)]
        public int Quantity { get; set; }

        [StringLength(InventoryModelsConstants.MAX_NOTES_LENGTH, MinimumLength = InventoryModelsConstants.MIN_NOTES_LENGTH)]
        public string Notes { get; set; }

        [StringLength(InventoryModelsConstants.MAX_DESCRIPTION_LENGTH)]
        public string Description { get; set; }

        public bool IsOnSale { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public DateTime? SoldDate { get; set; }

        [Range(InventoryModelsConstants.MINIMUM_PRICE, InventoryModelsConstants.MAXIMUM_PRICE)]
        public decimal? CurrentOrFinalPrice { get; set; }

        [Range(InventoryModelsConstants.MINIMUM_PRICE, InventoryModelsConstants.MAXIMUM_PRICE)]
        public decimal? PurchasePrice { get; set; }

        [ForeignKey(nameof(Category))]
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; } 

        public virtual List<Player> Players { get; set; } = new List<Player>();



    }
}