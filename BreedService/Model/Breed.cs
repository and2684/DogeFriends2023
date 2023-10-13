namespace BreedService.Model
{
    public class Breed
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Characteristics { get; set; }
        public List<string>? Images { get; set; }
        public SizeEnum Size { get; set; }
        public CoatEnum Coat { get; set; }
    }

    public enum SizeEnum
    {
        Small,
        Medium,
        Big
    }

    public enum CoatEnum
    {
        Long,
        Medium,
        Short
    }
}
