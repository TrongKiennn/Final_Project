using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Model
{
    public class Event : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Date { get; set; }
        public string Time {  get; set; }
        public int TableNumber { get; set; }
        public string Type { get; set; }
        public string Note {  get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
