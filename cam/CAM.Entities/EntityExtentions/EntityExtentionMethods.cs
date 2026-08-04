using CAM.Entities.Models.Base;
//using CAM.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CAM.Entities.EntityExtentions
{
    public static class EntityExtentionMethods
    {
        public const string GETDATE = "(GETUTCDATE())";
        public const string DELETED = "0";
        public static void AddAuditableDefaults<TAuditableEntity>(this ModelBuilder modelBuilder) where TAuditableEntity : AuditableEntity
        {
            if (modelBuilder == null)
                throw new ArgumentNullException(nameof(modelBuilder));
            modelBuilder.Entity<TAuditableEntity>(entity =>
            {
                entity.Property(e => e.CreationDate)
                    .HasDefaultValueSql(GETDATE);
                entity.Property(e => e.ModificationDate)
                   .HasDefaultValueSql(GETDATE);
                entity.Property(e => e.Deleted)
                    .HasDefaultValueSql(DELETED);
            });
        }

        public static void AddAuditableDefaultForAllCompatibleEntities(this ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException(nameof(modelBuilder));
            foreach (var auditableEntity in modelBuilder.Model.GetEntityTypes().Where(e => e.ClrType.IsBaseEntity<AuditableEntity>()))
            {
                var method = typeof(EntityExtentionMethods).GetTypeInfo().DeclaredMethods
                    .Single(m => m.Name == nameof(AddAuditableDefaults));
                method.MakeGenericMethod(auditableEntity.ClrType).Invoke(null, new[] { modelBuilder });
            }
        }

        //public static void AddRestrictDeleteBehaviorToAllEntitiesExceptIdentity(this ModelBuilder modelBuilder)
        //{
        //    var identiyEntities = new List<IMutableEntityType>
        //    {
        //        modelBuilder.Model.FindEntityType(typeof(ApplicationUser)),
        //        modelBuilder.Model.FindEntityType(typeof(ApplicationRole)),
        //        modelBuilder.Model.FindEntityType(typeof(ApplicationUserRole)),
        //        modelBuilder.Model.FindEntityType(typeof(IdentityUserClaim<int>)),
        //        modelBuilder.Model.FindEntityType(typeof(IdentityUserLogin<int>)),
        //        modelBuilder.Model.FindEntityType(typeof(IdentityUserToken<int>)),
        //        modelBuilder.Model.FindEntityType(typeof(IdentityRoleClaim<int>)),
        //    };
        //    foreach (var relationship in modelBuilder.Model.GetEntityTypes()
        //        .Except(identiyEntities)
        //        .SelectMany(e => e.GetForeignKeys()))
        //    {
        //        relationship.DeleteBehavior = DeleteBehavior.Restrict;
        //    }
        //}
    }
}
