using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> UpdateAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Nullable<DateTime> CreatedAt { get; set; }
    }
}
