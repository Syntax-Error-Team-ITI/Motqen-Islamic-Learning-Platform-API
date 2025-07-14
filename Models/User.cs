namespace MotqenIslamicLearningPlatform_API.Models
{
    public class User : BaseEntity
    {

        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual Student? Student { get; set; }
        public virtual Teacher? Teacher { get; set; }
        public virtual Parent? Parent { get; set; }

    }
}
