namespace IctFinalProject.Models.DTOs
{
    public class CreateProjectParameter
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public decimal Price { get; set; }

        public string Category { get; set; }
    }
}