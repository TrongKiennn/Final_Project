using Xunit;
using Moq;
using POS_App.ViewModel;
using POS_App.Service.DataAccess;
using POS_App.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace POS_App.Test.ViewModel
{
    public class OrderPageViewModelTests
    {
        private readonly Mock<IDao> _daoMock;
        private readonly OrderPageViewModel _viewModel;

        public OrderPageViewModelTests()
        {
            _daoMock = new Mock<IDao>();
            _viewModel = new OrderPageViewModel
            {
                _dao = _daoMock.Object
            };
        }

        [Fact]
        public void LoadData_ShouldPopulateDrinksCollection()
        {
            // Arrange
            var drinks = new List<Drinks>
            {
                new Drinks { id = 1, name = "Coke", price = 1 },
                new Drinks { id = 2, name = "Pepsi", price = 1 }
            };
            _daoMock.Setup(d => d.GetDrink(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Dictionary<string, IDao.SortType>>(), It.IsAny<string>()))
                    .Returns(new Tuple<List<Drinks>, int>(drinks, drinks.Count));

            // Act
            _viewModel.LoadData();

            // Assert
            Assert.Equal(2, _viewModel.Drinks.Count);
            Assert.Equal("Coke", _viewModel.Drinks[0].name);
            Assert.Equal("Pepsi", _viewModel.Drinks[1].name);
        }

        [Fact]
        public void ExecuteFilter_ShouldUpdateTypeNameAndLoadData()
        {
            // Arrange
            var parameter = "Juice";

            // Act
            _viewModel.ExecuteFilter(parameter);

            // Assert
            Assert.Equal("Juice", _viewModel.typeName);
            _daoMock.Verify(d => d.GetDrink(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Dictionary<string, IDao.SortType>>(), "Juice"), Times.Once);
        }

        [Fact]
        public void SortById_ShouldUpdateSortOptionsAndLoadData()
        {
            // Act
            _viewModel.SortById = true;

            // Assert
            Assert.True(_viewModel.SortById);
            Assert.Contains("Name", _viewModel._sortOptions.Keys);
            _daoMock.Verify(d => d.GetDrink(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), _viewModel._sortOptions, It.IsAny<string>()), Times.Once);
        }
    }
}
