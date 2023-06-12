using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharper.Models
{
   
     public class Book
      {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Experience { get; set; }
        public string? LocalLink { get; set; }
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

        public Reading Reading
        {
            get { return this.reading(); }
            set
              {
                this.setReading(value);
               
              }
        }

    }

    public static class BookExtensions
    {
        private static User user;

        public static void SetCurrentUser(this Book book,User _user)
        {
            user = _user;
        }

        public static Reading reading(this Book book)
        {
            if(book.Users.Contains(user))
               return Reading.Yes;

            return Reading.No;
        }
        public static void setReading(this Book book, Reading r)
        {
            
            if (r !=0) book?.Users.Add(user);
            else if (book?.Users.Contains(user)==true)
                book.Users.Remove(user);
            book.Name = "ttt"; //БАГ!!!  Это изменение не отражается на форме
        }
    }


}
