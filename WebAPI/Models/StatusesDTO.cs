namespace WebAPI.Models
{
    public class StatusesDTO
    {
        public string? version { get; set; }
        public int? responseCode { get; set; }
        public string? responseMessage { get; set; }  // Cambia string[]? a string?
    }
}