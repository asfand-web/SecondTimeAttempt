using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecondTimeAttempt.Models.Domain
{
    public abstract class BaseModel<TKey>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey Id { get; set; }

        public DateTime Created_at { get; set; } = DateTime.UtcNow;

        public DateTime? Updated_at { get; set; }
    }
}
