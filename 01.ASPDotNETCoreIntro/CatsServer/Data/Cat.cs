namespace CatsServer.Data
{
    using System.ComponentModel.DataAnnotations;

    public class Cat
    {
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Range(0, 30)]
        public int Age { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string Breed { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(2000)]
        public string ImageUrl { get; set; }
    }
}