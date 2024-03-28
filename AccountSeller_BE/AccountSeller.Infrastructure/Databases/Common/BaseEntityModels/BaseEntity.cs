using AccountSeller.Infrastructure.Databases.Common.TableColumnAudit;

namespace AccountSeller.Infrastructure.Databases.Common.BaseEntityModels
{
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the insert user identifier.
        /// </summary>
        public Guid? InsertUserId { get; set; }

        /// <summary>
        /// Gets or sets the insert date.
        /// </summary>
        [ItemId("0000")]
        public DateTime? InsertDate { get; set; }

        /// <summary>
        /// Gets or sets the update user identifier.
        /// </summary>
        public Guid? UpdateUserId { get; set; }

        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        [ItemId("0000")]
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the delete user identifier.
        /// </summary>
        public Guid? DeleteUserId { get; set; } = null;

        /// <summary>
        /// Gets or sets the delete date.
        /// </summary>
        [ItemId("0000")]
        public DateTime? DeleteDate { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        public bool? IsDeleted { get; set; } = false;
    }
}
