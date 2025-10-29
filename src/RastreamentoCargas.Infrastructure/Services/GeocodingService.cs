using RastreamentoCargas.Application.DTOs.Coordinates;
using RastreamentoCargas.Application.Interfaces;
using System.Text.Json;

namespace RastreamentoCargas.Infrastructure.Services
{
    public sealed class GeocodingService(HttpClient httpClient) : IGeocodingService
    {
        private record NominatimResult(string Lat, string Lon);

        public async Task<CoordinatesDto> GetCoordinatesAsync(string address)
        {
            var encodedAddress = Uri.EscapeDataString(address);

            var uri = $"search?q={encodedAddress}&format=json&limit=1";

            var response = await httpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            var results = JsonSerializer.Deserialize<NominatimResult[]>(jsonString);

            if (results == null || results.Length == 0)
            {
                throw new InvalidOperationException($"Não foi possível geocodificar o local: {address}");
            }

            var bestResult = results.First();

            return new CoordinatesDto(
                Latitude: double.Parse(bestResult.Lat, System.Globalization.CultureInfo.InvariantCulture),
                Longitude: double.Parse(bestResult.Lon, System.Globalization.CultureInfo.InvariantCulture)
            );
        }
    }
}