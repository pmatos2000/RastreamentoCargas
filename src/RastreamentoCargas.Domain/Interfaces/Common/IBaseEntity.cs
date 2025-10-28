using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RastreamentoCargas.Domain.Interfaces.Common
{
    public interface IBaseEntity
    {
        public long Id { get; set; }
        public Guid ExternalId { get; set; }
    }
}
