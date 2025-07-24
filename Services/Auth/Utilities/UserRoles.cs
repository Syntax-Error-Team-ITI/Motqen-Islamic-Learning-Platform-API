namespace MotqenIslamicLearningPlatform_API.Services.Auth.Utilities
{
    public static class UserRoles
    {
        /*
         * Const Property Means:
         * readonly (implicitly): cannot be modified at runtime
         * must be initialized at declaration
         * static (implicitly): cannot be instantiated (no object of this class can be created)
         * No memory allocation at runtime (value is compiled into the code, not variable in memory)
         */

        public const string Admin = "Admin";
        public const string Teacher = "Teacher";
        public const string Student = "Student";
        public const string Parent = "Parent";
        //public const string Guest = "Guest";
    }
}
