using InventoryModels.Interfaces;

namespace InventoryModels
{
    public abstract class FullAuditModel : IIdentityModel, IActivatableModel, IAuditedModel
    {
        public int Id { get; set; }
        public bool IsActive { get;  set ; }
        public string CreateByUserId { get ; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedUserId { get; set; }
        public DateTime? LastModifiedDate { get; set ; }
    }
}
