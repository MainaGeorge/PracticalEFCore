using InventoryModels.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryModels
{
    [Table("ItemGenres")]
    [Index(nameof(ItemId), nameof(GenreId), IsUnique =true)]
    public class ItemGenre : IIdentityModel
    {
        public int Id { get; set; }
        public virtual int ItemId { get; set; }
        public virtual int GenreId { get; set; }

        public virtual Genre Gender { get; set; }
        public virtual Item Item { get; set; }
    }
}
