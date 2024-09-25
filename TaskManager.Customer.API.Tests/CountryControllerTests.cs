using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TaskManager.Customer.API.Controllers;
using TaskManager.Customer.API.Models;
using TaskManager.Customer.API.Repositories.Abstractions;

namespace TaskManager.Customer.API.Tests
{
    [TestFixture]
    internal class CountryControllerTests
    {
        private Mock<ICountryRepository> _countryRepositoryMock;
        private Mock<ILogger<CountryController>> _loggerMock;

        private CountryController _countryControllerMock;

        private readonly Country testCountry = new()
        {
            ID = 1,
            ISOCode = "IE",
            Name = "Ireland"
        };

        [SetUp]
        public void Setup()
        {
            _countryRepositoryMock = new Mock<ICountryRepository>();
            _loggerMock = new Mock<ILogger<CountryController>>();

            _countryControllerMock = new CountryController(_countryRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task When_A_Country_Is_Searched_For_By_Name_And_It_Exists_In_The_Repository_Then_The_Country_With_The_Same_Name_As_The_Search_Term_Should_Be_Returned()
        {
            //Arrange
            _countryRepositoryMock.Setup(x => x.GetCountryByNameAsync(testCountry.Name)).ReturnsAsync(testCountry);

            //Act 
            var result = await _countryControllerMock.Get(testCountry.Name);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(OkObjectResult)));

            var objectResult = ((OkObjectResult)result).Value;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(((Country)objectResult).Name, Is.EqualTo(testCountry.Name));
        }

        [Test]
        public async Task When_A_Country_Is_Searched_For_By_Name_And_It_Does_Not_Exist_In_The_Repository_Then_A_Not_Found_Response_Should_Be_Returned()
        {
            //Arrange
            _countryRepositoryMock.Setup(x => x.GetCountryByNameAsync(testCountry.Name)).ReturnsAsync(testCountry);

            //Act 
            string searchCountryName = "test";
            var result = await _countryControllerMock.Get(searchCountryName);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(NotFoundResult)));
        }

        [Test]
        public async Task When_A_Country_Is_Searched_For_By_Country_Code_And_It_Exists_In_The_Repository_Then_The_Country_With_The_Same_Country_Code_As_The_Search_Term_Should_Be_Returned()
        {
            //Arrange
            _countryRepositoryMock.Setup(x => x.GetCountryByCountryCodeAsync(testCountry.ISOCode)).ReturnsAsync(testCountry);

            //Act 
            var result = await _countryControllerMock.Get(testCountry.ISOCode);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(OkObjectResult)));

            var objectResult = ((OkObjectResult)result).Value;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(((Country)objectResult).Name, Is.EqualTo(testCountry.Name));
        }

        [Test]
        public async Task When_A_Country_Is_Searched_For_By_Country_Code_And_It_Does_Not_Exist_In_The_Repository_Then_A_Not_Found_Response_Should_Be_Returned()
        {
            //Arrange
            _countryRepositoryMock.Setup(x => x.GetCountryByCountryCodeAsync(testCountry.ISOCode)).ReturnsAsync(testCountry);

            //Act 
            string searchCountryName = "te";
            var result = await _countryControllerMock.Get(searchCountryName);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(NotFoundResult)));
        }

        [Test]
        public async Task When_A_Country_Is_Searched_For_Using_A_Empty_String_Then_A_Bad_Request_Response_Should_Be_Returned()
        {
            //Arrange
            _countryRepositoryMock.Setup(x => x.GetCountryByNameAsync(testCountry.Name)).ReturnsAsync(testCountry);

            //Act 
            string searchCountryName = string.Empty;
            var result = await _countryControllerMock.Get(searchCountryName);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(BadRequestObjectResult)));
        }

        [Test]
        public async Task When_A_Country_Is_Searched_For_By_Id_And_It_Exists_In_The_Repository_Then_The_Country_With_The_Same_Id_As_The_Search_Term_Should_Be_Returned()
        {
            //Arrange
            _countryRepositoryMock.Setup(x => x.GetCountryByIDAsync(testCountry.ID)).ReturnsAsync(testCountry);

            //Act 
            var result = await _countryControllerMock.Get(testCountry.ID);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(OkObjectResult)));

            var objectResult = ((OkObjectResult)result).Value;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(((Country)objectResult).ID, Is.EqualTo(testCountry.ID));
        }

        [Test]
        public async Task When_A_Country_Is_Searched_For_By_Id_And_It_Does_Not_Exist_In_The_Repository_Then_A_Not_Found_Response_Should_Be_Returned()
        {
            //Arrange
            _countryRepositoryMock.Setup(x => x.GetCountryByIDAsync(testCountry.ID)).ReturnsAsync(testCountry);

            //Act 
            int searchCountryId = 2;
            var result = await _countryControllerMock.Get(searchCountryId);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(NotFoundResult)));
        }

        [Test]
        public async Task When_A_Country_Is_Searched_For_Using_A_Negative_Number_For_Id_Then_A_Bad_Request_Response_Should_Be_Returned()
        {
            //Arrange
            _countryRepositoryMock.Setup(x => x.GetCountryByIDAsync(testCountry.ID)).ReturnsAsync(testCountry);

            //Act 
            int searchCountryId = -1;
            var result = await _countryControllerMock.Get(searchCountryId);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(BadRequestObjectResult)));
        }
    }
}
