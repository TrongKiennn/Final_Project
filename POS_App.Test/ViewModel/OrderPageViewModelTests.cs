using Moq;
using POS_App;
using POS_App.Model;
using POS_App.Service.DataAccess;
using System;
using System.Collections.Generic;
using Xunit;

namespace POS_App.Tests
{
    public class OrderPageViewModelTests
    {
       
        [Fact]
        public void LoadData_ShouldPopulateDrinksCollection()
        {
            // Arrange
             

             var mockRepository = new Mock<IDao_Drinks_Test>();
            var testDrinks = new List<Drinks>
            {
                new Drinks { id = 1, name = "Espresso", price = 5, imageUrl = "/espresso.jpg" },
                new Drinks { id = 2, name = "Latte", price = 6, imageUrl = "/latte.jpg" }
            };

            mockRepository.Setup(repo => repo.GetDrink(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Dictionary<string, IDao.SortType>>(), null))
                .Returns(new Tuple<List<Drinks>, int>(testDrinks, testDrinks.Count));

            var viewModel = new OrderPageViewModel(mockRepository.Object);

            // Act
            viewModel.LoadData();

            // Assert
            Assert.Equal(2, viewModel.Drinks.Count);
            Assert.Equal("Espresso", viewModel.Drinks[0].name);
            Assert.Equal("Latte", viewModel.Drinks[1].name);
        }

        [Fact]
        public void Search_ShouldTriggerLoadData()
        {
            // Arrange
            var mockRepository = new Mock<IDao_Drinks_Test>();
            var testDrinks = new List<Drinks>
            {
                new Drinks { id = 1, name = "Mocha", price = 7, imageUrl = "/mocha.jpg" }
            };

            mockRepository.Setup(repo => repo.GetDrink(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Dictionary<string, IDao.SortType>>(), null))
                .Returns(new Tuple<List<Drinks>, int>(testDrinks, testDrinks.Count));

            var viewModel = new OrderPageViewModel(mockRepository.Object)
            {
                Keyword = "Mocha"
            };

            // Act
            viewModel.SearchCommand.Execute(null);

            // Assert
            Assert.Single(viewModel.Drinks);
            Assert.Equal("Mocha", viewModel.Drinks[0].name);
        }

        [Fact]
        public void sortByName_ShouldSortDrinksByName()
        {
            // Arrange
            var mockRepository = new Mock<IDao_Drinks_Test>();
            var testDrinks = new List<Drinks>
            {
                new Drinks { id = 1, name = "Espresso", price = 5, imageUrl = "/espresso.jpg" },
                new Drinks { id = 2, name = "An", price = 6, imageUrl = "/latte.jpg" }
            };

            mockRepository.Setup(repo => repo.GetDrink(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.Is<Dictionary<string, IDao.SortType>>(sortOptions => sortOptions.ContainsKey("Name") && sortOptions["Name"] == IDao.SortType.Ascending), null))
                .Returns(new Tuple<List<Drinks>, int>(new List<Drinks>
                {
                    new Drinks { id = 2, name = "An", price = 6, imageUrl = "/latte.jpg" },
                    new Drinks { id = 1, name = "Espresso", price = 5, imageUrl = "/espresso.jpg" }
                }, 2));

            var viewModel = new OrderPageViewModel(mockRepository.Object);

            // Act
            viewModel.SortByNameCommand.Execute(null);

            // Assert
            Assert.Equal(2, viewModel.Drinks.Count);
            Assert.Equal("An", viewModel.Drinks[0].name);
            Assert.Equal("Espresso", viewModel.Drinks[1].name);
        }
    }
}
