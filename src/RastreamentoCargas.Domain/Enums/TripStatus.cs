using System.ComponentModel;

namespace RastreamentoCargas.Domain.Enums
{
    public enum TripStatus
    {
        [Description("Iniciada")]
        Initiated = 1,

        [Description("Em Trânsito")]
        InTransit = 2,

        [Description("Transbordo")]
        Transshipment = 3,

        [Description("Entregue")]
        Delivered = 4,

        [Description("Cancelada")]
        Canceled = 5
    }
}
