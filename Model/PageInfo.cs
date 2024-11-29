using System.ComponentModel;

namespace DemoListBinding1610;

public class PageInfo: INotifyPropertyChanged {
    public int Page {  get; set; }
    public int Total { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
}

