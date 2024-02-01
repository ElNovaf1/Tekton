using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Tekton.Domain.Entities
{
    public interface IEntityBase<TId>
    {
        TId Id { get; set; }
    }

    public abstract class EntityBase<TId> : IEntityBase<TId>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TId Id { get; set; }
    }
}
