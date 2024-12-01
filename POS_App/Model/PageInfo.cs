using System.ComponentModel;

namespace POS_App;

public class PageInfo : INotifyPropertyChanged
{
    public int Page { get; set; }
    public int Total { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
}
