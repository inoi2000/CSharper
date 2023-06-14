using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CSharper.Models;
using Wpf.Ui.Common.Interfaces;
using System;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSharper.ViewModels
{
    public partial class AssignmentsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private ObservableCollection<Assignment> AssignmentsFromDB;

        [ObservableProperty]
        private User _currentUser;

        [ObservableProperty]
        private ObservableCollection<Assignment> _assignments;

        [ObservableProperty]
        private List<string> _AssignmentThemes;

        [ObservableProperty]
        private Assignment _selectAssignment;

        [ObservableProperty]
        private Dictionary<string, RelayCommand> _selectCommands;

        public System.Windows.LineBreakCondition BreakAfter { get; }

        public RelayCommand SelectViewAllAssignmentCommand => new RelayCommand(() => { FromDB("Задание"); });//, "Все"
        public RelayCommand SelectViewNewAssignmentCommand => new RelayCommand(() => { FromDB("1"); });//, "Новые"
        public RelayCommand SelectViewNoReadAssignmentCommand => new RelayCommand(() => { FromDB("2"); });//, "Непрочитанные"
        public RelayCommand SelectViewBestAssignmentCommand => new RelayCommand(() => { FromDB("3"); });//, "С высоким рейтингом"

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
            {
                InitializeViewModel();
            }
        }

        public void OnNavigatedFrom()
        {
        }

        public void FromDB(string t)
        {
            //Books.Clear();
            //BooksFromDB.ToList().ForEach(x => Books.Add(x));

            //BooksFromDB.ToList().FindAll(x => !x.Name.Contains(t)).ForEach(x=>Books.Remove(x));
        }

        public AssignmentsViewModel()
        {
            InitializeViewModel();
        }
        private void InitializeViewModel()
        {
            AssignmentsFromDB = new ObservableCollection<Assignment>();
            AssignmentsFromDB.Add(new Assignment() { Name = "Задание1",
                Description= "Из города а в город б отправились 2 машины...",
                LocalLink = "DownloadBooks/books.pdf",Complexity = Complexity.easy });

            AssignmentsFromDB.Add(new Assignment() { Name = "Задание2",
                Description = "Из города а в город б отправились 2 машины...",
                LocalLink = "DownloadBooks/books.pdf", Complexity = Complexity.easy });

            AssignmentsFromDB.Add(new Assignment() { Name = "Задание3",
                Description = "Из города а в город б отправились 2 машины 'sd; lgk'sd" +
                ";lk gs'dg;l ks'd;lkg's d;lgk'sd ;lgk'sd;l kg'ds;l kg'sd ;lkg's;ldgk 'sd;lkg's ;ldk" +
                "gs; ldkg';s ldkg';ds lkg;ls dkg';lk dg';ldkg'l ;dgsdg dsgsd gsd gsdfgs dgsdgds gsdg" +
                "sdgs dgs dgsdd ddddd dddddd ddddddd ddddd dddd ddd dddd ddd ddd dddd ddd ddddd dddd ddd ddd" +
                "fgg ggg gg gggg gggg gggggg gg gggg ggg gggg gggg ggggg gggg ggggg ggggjdddddddddddf" +
                "gggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg" +
                "ggdddddddddddddddddddddhggggggggggggggggggggggggggggggggggggggggggggggggggdgk",
                LocalLink = "DownloadBooks/books.pdf", Complexity = Complexity.easy });

            AssignmentsFromDB.Add(new Assignment() { Name = "Задание4",
                Description = "Из города а в город б отправились 2 машины...",
                LocalLink = "DownloadBooks/books.pdf", Complexity = Complexity.easy });

            AssignmentsFromDB.Add(new Assignment()
            {
                Name = "Задание5",
                Description = "Из города а в город б отправились 2 машины...",
                LocalLink = "DownloadBooks/books.pdf",
                Complexity = Complexity.easy
            });

            AssignmentsFromDB.Add(new Assignment()
            {
                Name = "Задание6",
                Description = "Из города а в город б отправились 2 машины...",
                LocalLink = "DownloadBooks/books.pdf",
                Complexity = Complexity.easy
            });

            AssignmentsFromDB.Add(new Assignment()
            {
                Name = "Задание7",
                Description = "Из города а в город б отправились 2 машины...",
                LocalLink = "DownloadBooks/books.pdf",
                Complexity = Complexity.easy
            });



            Assignments = AssignmentsFromDB;
            //Books = new ObservableCollection<Book>();
            //BooksFromDB.ToList().ForEach(x=>Books.Add(x));

            CurrentUser = new User() { Id = Guid.NewGuid() };

            //[NotMapped]
            Assignments[0].Users.Add(CurrentUser);
            Assignments[2].Users.Add(CurrentUser);
            Assignments[3].Users.Add(CurrentUser);
            Assignments[0].SetCurrentUser(CurrentUser);

            var assignmentThemes = new List<string>();
            assignmentThemes.Add("Все");
            assignmentThemes.Add("Тема 1");
            assignmentThemes.Add("Тема 2");
            assignmentThemes.Add("Тема 3");
            assignmentThemes.Add("Тема 4");

            AssignmentThemes = assignmentThemes;

            //var selectCommands = new Dictionary<string,RelayCommand>();
            //selectCommands.Add("Все",SelectViewAllBookCommand);
            //selectCommands.Add("1",SelectViewNewBookCommand);
            //selectCommands.Add("2",SelectViewNoReadBookCommand);
            //selectCommands.Add("3",SelectViewBestBookCommand);

            //SelectCommands = selectCommands;
            _isInitialized = true;
        }


    }
}
