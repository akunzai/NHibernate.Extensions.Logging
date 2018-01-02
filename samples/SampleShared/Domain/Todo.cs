using System;
using System.ComponentModel.DataAnnotations;

namespace SampleShared.Domain
{
    public class Todo
    {
        public virtual Guid Id { get; set; }

        [Required]
        public virtual string Title { get; set; }

        public virtual bool Completed { get; set; }

        public virtual DateTime Created { get; set; } = DateTime.Now;
    }
}
