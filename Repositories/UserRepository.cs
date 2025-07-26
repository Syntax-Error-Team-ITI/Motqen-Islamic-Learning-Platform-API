using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.DTOs.UserDTOs;
using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.Shared;

namespace MotqenIslamicLearningPlatform_API.Repositories
{
    public class UserRepository(
        MotqenDbContext db,
        UserManager<User> userManager
        )
        : GenericRepository<User>(db)
    {
        public async Task<UserDisplayDTO?> GetById(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null) return null;

            var roles = await userManager.GetRolesAsync(user);

            return new UserDisplayDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles.ToArray(),
                IsDeleted = user.IsDeleted
            };
        }

        public async Task<ICollection<UserDisplayDTO>?> GetAll()
        {
            var users = await userManager.Users.ToListAsync();
            if (users == null || users.Count == 0)
                return null;

            var displayUsers = new List<UserDisplayDTO>();

            foreach (var user in users)
            {
                ICollection<string>? roles = null;

                if (await userManager.GetRolesAsync(user) != null)
                {
                    roles = await userManager.GetRolesAsync(user);
                }

                displayUsers.Add(new UserDisplayDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = roles?.ToArray(),
                    IsDeleted = user.IsDeleted
                });
            }
            return displayUsers;
        }

        public async Task<bool?> SoftDelete(string id)
        {
            var user = await db.Users
                .Include(u => u.Student)
                .Include(u => u.Teacher)
                .Include(u => u.Parent)
                .SingleOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;
            if (user.IsDeleted)
                return false;

            user.IsDeleted = true;
            if (user.Teacher != null)
                user.Teacher.IsDeleted = true;
            if (user.Student != null)
                user.Student.IsDeleted = true;
            if (user.Parent != null)
                user.Parent.IsDeleted = true;

            var result = await userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool?> Restore(string id)
        {
            var user = await db.Users
                .Include(u => u.Student)
                .Include(u => u.Teacher)
                .Include(u => u.Parent)
                .SingleOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;
            if (!user.IsDeleted)
                return false;

            user.IsDeleted = false;
            if (user.Teacher != null)
                user.Teacher.IsDeleted = false;
            if (user.Student != null)
                user.Student.IsDeleted = false;
            if (user.Parent != null)
                user.Parent.IsDeleted = false;

            var result = await userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> HardDelete(string id)
        {
            var user = await db.Users
                           .Include(u => u.Student)
                           .Include(u => u.Teacher)
                           .Include(u => u.Parent)
                           .SingleOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return false;

            if (user.Teacher != null)
                Db.Teachers.Remove(user.Teacher);
            if (user.Student != null)
                Db.Students.Remove(user.Student);
            if (user.Parent != null)
                Db.Parents.Remove(user.Parent);
            Db.Users.Remove(user);

            var result = await db.SaveChangesAsync();

            return result == 1 ? false : true;
        }

    }
}
