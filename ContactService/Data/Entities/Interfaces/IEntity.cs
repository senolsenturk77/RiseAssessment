using System;

namespace ContactService.Data.Entities
{
    public interface IEntity
    {
        public Guid Uuid { get; set; }

    }
}
