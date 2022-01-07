using System.ComponentModel.DataAnnotations;

namespace InventoryModels
{
    public class Genre : FullAuditModel
    {
        [Required, StringLength(InventoryModelsConstants.MAX_GENRENAME_LENGTH)]
        public string Name { get; set; }

        public List<ItemGenre> ItemGenres { get; set; } = new List<ItemGenre>();
    }
}
