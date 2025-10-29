using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RastreamentoCargas.Application.DTOs.Trips
{
    public record UpdateLocationRequestDto
    {
        [Required(ErrorMessage = "A nova localização é obrigatória.")]
        public required string NewLocation { get; init; }
    }
}
