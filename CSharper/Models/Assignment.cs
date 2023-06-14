using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CSharper.Views.Pages;



namespace CSharper.Models
{
    public class Assignment
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Experience { get; set; }
        public string LocalLink { get; set; }
        public Uri? Url { get; set; }
        public Complexity? Complexity { get; set; }

        public Subject Subject { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();






        public override string ToString()
        {
            return Name;
        }

        //public override bool Equals(object? obj)
        //{
        //    if (obj == null) throw new ArgumentNullException(nameof(obj));

        //    if (obj is Book book)
        //    {
        //        if (this.Id == book.Id) { return true; }
        //        else { return false; }
        //    }
        //    else throw new ArgumentException(nameof(obj));
        //}

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [NotMapped]
        public Reading Reading
        {
            get { return this.reading(); }
            set
            {
                this.setReading(value);

            }
        }

    }

    public static class AssignmentExtensions
    {
        private static User user;

        public static void SetCurrentUser(this Assignment assignment, User _user)
        {
            user = _user;
        }

        public static Reading reading(this Assignment assignment)
        {
            if (assignment.Users.Contains(user))
                return Reading.Yes;

            return Reading.No;
        }
        public static void setReading(this Assignment assignment, Reading r)
        {

            if (r != 0) assignment?.Users.Add(user);
            else if (assignment?.Users.Contains(user) == true)
                assignment.Users.Remove(user);
            assignment.Name = "ttt"; //БАГ!!!  Это изменение не отражается на форме
        }
    }



}

