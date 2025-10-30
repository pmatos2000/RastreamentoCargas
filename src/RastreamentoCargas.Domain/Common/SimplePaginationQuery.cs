using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RastreamentoCargas.Domain.Common
{
    public record SimplePaginationQuery
    {
        private const int MaxPageSize = 50;
        private const int DefaultPageSize = 10;

        [Range(1, MaxPageSize)]
        public int PageSize { get; init; } = DefaultPageSize;

        [Range(1, int.MaxValue)]
        public int PageNumber { get; init; } = 1;

        public bool SortAsc { get; init; } = false;
    }
}
