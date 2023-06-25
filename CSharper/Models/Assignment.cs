using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CSharper.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace CSharper.Models
{
    public class Assignment : IPdfReading
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Experience { get; set; }
        public string? LocalLink { get; set; }
        public Uri? Url { get; set; }
        public Complexity Complexity { get; set; }

        public Subject Subject { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            if (obj is Assignment assignment)
            {
                if (this.Id == assignment.Id) { return true; }
                else { return false; }
            }
            else return false;
        }

        [NotMapped]
        public bool Reading => Users.Contains(AppConfig.User);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }

    public static class AssignmentExtensions
    {
        private static User user;

        public static void SetCurrentUser(this Book assignment, User _user)
        {
            user = _user;
        }

        public static Reading reading(this Book assignment)
        {
            if (assignment.Users.Contains(user))
                return Reading.Yes;

            return Reading.No;
        }
        public static void setReading(this Book assignment, Reading r)
        {

            if (r != 0) assignment?.Users.Add(user);
            else if (assignment?.Users.Contains(user) == true)
                assignment.Users.Remove(user);
            assignment.Name = "ttt"; //БАГ!!!  Это изменение не отражается на форме
        }
    }
}
