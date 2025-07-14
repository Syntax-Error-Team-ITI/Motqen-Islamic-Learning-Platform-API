namespace MotqenIslamicLearningPlatform_API.Models
{
    public class Parent : BaseEntity
    {
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Pic { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public IList<Student> Students { get; set; } = new List<Student>();
    }
}
