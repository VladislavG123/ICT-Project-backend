namespace IctFinalProject.Models.DTOs
{
    public class GetAllProductParameter
    {
        public bool ShowInactive { get; set; } = false;
        public string Category { get; set; } = "";
    }
}