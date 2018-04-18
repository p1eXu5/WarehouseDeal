using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGridDynamic.Models
{
    public class Student
    {
        public static int index { get; internal set; }
        public string StudentName { get; set; }
        public int StudentId { get; set; }
        public ObservableCollection<decimal> ProjectScores { get; set; }
        public decimal Score => ProjectScores[index];
    }
}
