using POS_App.Command;
using POS_App.Model;
using POS_App.Models;
using POS_App.Service;
using POS_App.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Services.Maps;
using static POS_App.Service.DataAccess.IDao;

namespace POS_App;

public partial class OrderPageViewModel : INotifyPropertyChanged
{
    IDao _dao;
    public ICommand FilterCommand { get; }
    public ObservableCollection<Drinks> Drinks { get; set; }
    public ObservableCollection<PageInfo> PageInfos { get; set; }
    public PageInfo SelectedPageInfoItem { get; set; }
    public OrderDetail OrderDetails { get; set; }
    public ICommand CancelCommand { get; }
    public string Keyword { get; set; } = "";

    public string typeName { get; set; } = null;
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; } = 0;
    public int RowsPerPage { get; set; }

    private bool _sortById = false;
    public bool SortById
    {
        get => _sortById;
        set
        {
            _sortById = value;
            if (value == true)
            {
                _sortOptions.Add("Name", SortType.Ascending);
            }
            else
            {
                if (_sortOptions.ContainsKey("Name"))
                {
                    _sortOptions.Remove("Name");
                }
            }

            LoadData();
        }
    }

    public OrderPageViewModel()
    {
        RowsPerPage = 8;
        CurrentPage = 1;
        FilterCommand = new RelayCommand(ExecuteFilter);
        OrderDetails = new OrderDetail();
        CancelCommand = new RelayCommand(ClearSelections);
        _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;

        LoadData();
    }
    private void ExecuteFilter(object parameter)
    {

        if (parameter is string type_Name)
        {
            typeName = type_Name=="All"?null:type_Name;
            CurrentPage = 1;
            LoadData();

        }
    }

    private Dictionary<string, SortType> _sortOptions = new();
    public event PropertyChangedEventHandler PropertyChanged;

    public void GoToNextPage()
    {
        if (CurrentPage < TotalPages)
        {
            CurrentPage++;
            LoadData();
        }
    }

    public void GoToPreviousPage()
    {
        if (CurrentPage > 1)
        {
            CurrentPage--;
            LoadData();
        }
    }

    public void LoadData()
    {

        var (items, count) = _dao.GetDrink(
            CurrentPage, RowsPerPage, Keyword,
            _sortOptions, typeName
        );
        Drinks = new ObservableCollection<Drinks>(
            items
        );

        if (count != TotalItems)
        {
            TotalItems = count;
            TotalPages = (TotalItems / RowsPerPage) +
                (((TotalItems % RowsPerPage) == 0) ? 0 : 1);

            PageInfos = new();
            for (int i = 1; i <= TotalPages; i++)
            {
                PageInfos.Add(new PageInfo
                {
                    Page = i,
                    Total = TotalPages
                });
            }
        }

        SelectedPageInfoItem = PageInfos[CurrentPage - 1];
    }

    public void GoToPage(int page)
    {
        CurrentPage = page;
        LoadData();
    }

    public void Search()
    {
        CurrentPage = 1;
        LoadData();
    }
    private void ClearSelections(object parameter)
    {
        OrderDetails.Sugar100 = false;
        OrderDetails.Sugar50 = false;
        OrderDetails.NoIce = false;
        OrderDetails.SeparateIce = false;
        OrderDetails.ShareIce = false;
        OrderDetails.LittleIce = false;
        OrderDetails.TakeAway = false;
        OrderDetails.StayHere = false;
        OrderDetails.Note = string.Empty;
    }
}