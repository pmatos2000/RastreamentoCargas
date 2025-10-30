using RastreamentoCargas.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RastreamentoCargas.Application.DTOs.Common
{
    public record PagedResponseDto<T>
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int TotalPages { get; init; }
        public int TotalCount { get; init; }
        public bool HasPreviousPage { get; init; }
        public bool HasNextPage { get; init; }
        public required IEnumerable<T> Items { get; init; }


        /// <summary>
        /// Cria um PagedResponseDto<T> a partir de um PagedList<U> e uma função de mapeamento.
        /// </summary>
        /// <typeparam name="U">O tipo dos itens na PagedList de origem.</typeparam>
        /// <param name="pagedList">A lista paginada de origem.</param>
        /// <param name="map">A função que mapeia um item de U para um item de T.</param>
        /// <returns>Um PagedResponseDto<T> totalmente preenchido.</returns>
        public static PagedResponseDto<T> Create<U>(PagedList<U> pagedList, Func<U, T> map)
        {
            var mappedItems = pagedList.Items.Select(map).ToList();

            return new PagedResponseDto<T>
            {
                Items = mappedItems,
                TotalCount = pagedList.TotalCount,
                PageNumber = pagedList.PageNumber,
                PageSize = pagedList.PageSize,
                TotalPages = pagedList.TotalPages,
                HasPreviousPage = pagedList.PageNumber > 1,
                HasNextPage = pagedList.PageNumber < pagedList.TotalPages
            };
        }
    }
}
