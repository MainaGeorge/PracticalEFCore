﻿using InventoryModels.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace InventoryModels
{
    public abstract class FullAuditModel : IIdentityModel, IActivatableModel, IAuditedModel
    {
        public int Id { get; set; }
        public bool IsActive { get;  set ; }

        [StringLength(InventoryModelsConstants.MAX_USERID_LENGTH)]
        public string CreateByUserId { get ; set; }
        public DateTime CreatedDate { get; set; }

        [StringLength(InventoryModelsConstants.MAX_USERID_LENGTH)]
        public string LastModifiedUserId { get; set; }
        public DateTime? LastModifiedDate { get; set ; }
    }
}