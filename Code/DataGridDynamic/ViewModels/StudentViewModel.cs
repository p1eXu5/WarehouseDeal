using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using DataGridDynamic.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace DataGridDynamic.ViewModels
{
    public class StudentViewModel : BindableBase
    {
        private ObservableCollection<Student> _studentList;
        private List<string> _titleList;

        public StudentViewModel()
        {
            PopulateStudents();
            ChangeSortedColumnCommand = new DelegateCommand<object>(p =>
            {
                string param = p as string;
                int ind = TitleList.IndexOf (param);
                if (ind > 0) Student.index = ind;
                StudentList[0].ProjectScores[0] = 34;
                Direction = ListSortDirection.Descending;
                Debug.WriteLine ((string)p);
            });
        }

        private ListSortDirection _direction = ListSortDirection.Ascending;
        public ListSortDirection Direction
        {
            get => _direction;
            set {
                _direction = value;
                RaisePropertyChanged ();
            }
        }

        public ObservableCollection<Student> StudentList
        {
            get => _studentList;
            set {
                _studentList = value;
                RaisePropertyChanged ();
            }
        }

        public List<string> TitleList
        {
            get => _titleList;
            set {
                _titleList = value;
                RaisePropertyChanged ();
            }
        }

        public DelegateCommand<object> ChangeSortedColumnCommand { get; }

        #region Methods
        /// <summary>
        /// Инициализация списка студентов с их оценками и списка названий тестов
        /// </summary>
        public void PopulateStudents()
        {
            var itemList = new ObservableCollection<Student>()
            {
                new Student()
                {
                    StudentId = 2,
                    StudentName = "Frodo Baggins",
                    ProjectScores = new ObservableCollection<decimal> {89M, 97M, 88M}
                },
                new Student()
                {
                    StudentId = 1,
                    StudentName = "Rosie Cotton",
                    ProjectScores = new ObservableCollection<decimal> {77M, 71M, 94M}
                },
                new Student()
                {
                    StudentId = 5,
                    StudentName = "Samwise Gamgee",
                    ProjectScores = new ObservableCollection<decimal> {83M, 90M, 90M}
                },
                new Student()
                {
                    StudentId = 4,
                    StudentName = "Peregrin Took",
                    ProjectScores = new ObservableCollection<decimal> {69M, 72M, 75M}
                },
            };

            StudentList = itemList;

            var itemNameList = new List<string> { "PreTest", "Chp 1", "Test" };

            TitleList = itemNameList;
        }
        #endregion
    }
}
