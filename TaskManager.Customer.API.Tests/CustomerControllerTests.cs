using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TaskManager.Customer.API.Controllers;
using TaskManager.Customer.API.Models;
using TaskManager.Customer.API.Repositories.Abstractions;

namespace TaskManager.Customer.API.Tests
{
    [TestFixture]
    internal class CustomerControllerTests
    {
        private Mock<ICountryRepository> _countryRepositoryMock;
        private Mock<ICustomerRepository> _customerRepositoryMock;
        private Mock<ILogger<CustomerController>> _loggerMock;

        private CustomerController _customerControllerMock;

        private Country testCountry = new()
        {
            ID = 1,
            ISOCode = "IE",
            Name = "Ireland"
        };

        private readonly Models.Customer testCustomer = new()
        {
            ID = 0,
            Email = "test@test.com",
            FirstName = "test",
            LastName = "test",
            CountryCodeId = 1,
            Country = null
        };

        [SetUp]
        public void Setup()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _countryRepositoryMock = new Mock<ICountryRepository>();
            _loggerMock = new Mock<ILogger<CustomerController>>();

            _customerControllerMock = new CustomerController(
                _customerRepositoryMock.Object, _countryRepositoryMock.Object, _loggerMock.Object);
        }


        [Test]
        public async Task When_A_Customer_Is_Searched_For_By_Id_And_They_Exist_In_The_Repository_Then_The_Customer_With_The_Same_Id_As_The_Search_Term_Should_Be_Returned()
        {
            //Arrange
            testCustomer.Country = testCountry;
            testCustomer.CountryCodeId = testCountry.ID;

            _customerRepositoryMock.Setup(x => x.GetCustomerByIdAsync(testCustomer.ID)).ReturnsAsync(testCustomer);

            //Act 
            var result = await _customerControllerMock.Get(testCustomer.ID);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(OkObjectResult)));

            var objectResult = ((OkObjectResult)result).Value;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(((Models.Customer)objectResult).ID, Is.EqualTo(testCustomer.ID));
        }

        [Test]
        public async Task When_A_Customer_Is_Searched_For_By_Id_And_It_Does_Not_Exist_In_The_Repository_Then_A_Not_Found_Response_Should_Be_Returned()
        {
            //Arrange
            testCustomer.Country = testCountry;
            testCustomer.CountryCodeId = testCountry.ID;

            _customerRepositoryMock.Setup(x => x.GetCustomerByIdAsync(testCustomer.ID)).ReturnsAsync(testCustomer);

            //Act 
            int searchCustomerId = 2;
            var result = await _customerControllerMock.Get(searchCustomerId);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(NotFoundResult)));
        }

        [Test]
        public async Task When_A_Customer_Is_Searched_For_Using_An_Id_Less_Than_Zero_Then_A_Bad_Request_Response_Should_Be_Returned()
        {
            //Arrange
            testCustomer.Country = testCountry;
            testCustomer.CountryCodeId = testCountry.ID;

            _customerRepositoryMock.Setup(x => x.GetCustomerByIdAsync(testCustomer.ID)).ReturnsAsync(testCustomer);

            //Act 
            int searchCustomerId = -1;
            var result = await _customerControllerMock.Get(searchCustomerId);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(BadRequestObjectResult)));
        }


        [Test]
        public async Task When_A_New_Customer_Wants_To_Be_Registered_On_The_System_And_Their_Email_Address_Is_Not_Associated_With_Another_Account_And_They_Have_All_Nessessary_Information_Then_An_Ok_Response_Should_Be_Returned()
        {
            //Arrange
            testCustomer.Country = testCountry;
            testCustomer.CountryCodeId = testCountry.ID;

            _customerRepositoryMock.Setup(x => x.CustomerExistsWithEmailAsync(testCustomer.Email)).ReturnsAsync(false);
            _customerRepositoryMock.Setup(x => x.RegisterNewCustomerAsync(testCustomer.FirstName, testCustomer.LastName, testCustomer.Email, testCountry)).ReturnsAsync(true);

            _countryRepositoryMock.Setup(x => x.GetCountryByCountryCodeAsync(testCountry.ISOCode)).ReturnsAsync(testCountry);

            //Act 
            var result = await _customerControllerMock.RegisterNewCustomer(new DataTransferObjects.CustomerDTO
            {
                CountryCode = testCountry.ISOCode,
                Email = testCustomer.Email,
                FirstName = testCustomer.FirstName,
                LastName = testCustomer.LastName
            });

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(OkResult)));
        }


        [Test]
        public async Task When_A_New_Customer_Wants_To_Be_Registered_On_The_System_And_Their_Email_Address_Is_Associated_With_Another_Account_And_They_Have_All_Nessessary_Information_Then_A_Bad_Request_Response_Should_Be_Returned()
        {
            //Arrange
            testCustomer.Country = testCountry;
            testCustomer.CountryCodeId = testCountry.ID;

            _customerRepositoryMock.Setup(x => x.CustomerExistsWithEmailAsync(testCustomer.Email)).ReturnsAsync(true);
            _customerRepositoryMock.Setup(x => x.RegisterNewCustomerAsync(testCustomer.FirstName, testCustomer.LastName, testCustomer.Email, testCountry)).ReturnsAsync(true);

            _countryRepositoryMock.Setup(x => x.GetCountryByCountryCodeAsync(testCountry.ISOCode)).ReturnsAsync(testCountry);

            //Act 
            var result = await _customerControllerMock.RegisterNewCustomer(new DataTransferObjects.CustomerDTO
            {
                CountryCode = testCountry.ISOCode,
                Email = testCustomer.Email,
                FirstName = testCustomer.FirstName,
                LastName = testCustomer.LastName
            });

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(BadRequestObjectResult)));
        }

        [Test]
        public async Task When_A_New_Customer_Wants_To_Be_Registered_On_The_System_And_Their_Email_Address_Is_Not_Associated_With_Another_Account_And_They_Have_All_Nessessary_Information_Except_Their_Country_Code_Is_Not_Supported_Then_A_Bad_Request_Response_Should_Be_Returned()
        {
            //Arrange
            testCustomer.Country = testCountry;
            testCustomer.CountryCodeId = testCountry.ID;

            _customerRepositoryMock.Setup(x => x.CustomerExistsWithEmailAsync(testCustomer.Email)).ReturnsAsync(false);
            _customerRepositoryMock.Setup(x => x.RegisterNewCustomerAsync(testCustomer.FirstName, testCustomer.LastName, testCustomer.Email, testCountry)).ReturnsAsync(true);

            _countryRepositoryMock.Setup(x => x.GetCountryByCountryCodeAsync(testCountry.ISOCode)).ReturnsAsync(testCountry);

            //Act 
            var result = await _customerControllerMock.RegisterNewCustomer(new DataTransferObjects.CustomerDTO
            {
                CountryCode = "UK",
                Email = testCustomer.Email,
                FirstName = testCustomer.FirstName,
                LastName = testCustomer.LastName
            });

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(BadRequestObjectResult)));
        }

        [Test]
        public async Task When_A_New_Customer_Wants_To_Be_Registered_On_The_System_And_Their_Email_Address_Is_Not_Associated_With_Another_Account_And_They_Have_All_Nessessary_Information_Except_Their_FirstName_Is_Not_Provided_Then_A_Bad_Request_Response_Should_Be_Returned()
        {
            //Arrange
            testCustomer.Country = testCountry;
            testCustomer.CountryCodeId = testCountry.ID;

            _customerRepositoryMock.Setup(x => x.CustomerExistsWithEmailAsync(testCustomer.Email)).ReturnsAsync(false);
            _customerRepositoryMock.Setup(x => x.RegisterNewCustomerAsync(testCustomer.FirstName, testCustomer.LastName, testCustomer.Email, testCountry)).ReturnsAsync(true);

            _countryRepositoryMock.Setup(x => x.GetCountryByCountryCodeAsync(testCountry.ISOCode)).ReturnsAsync(testCountry);

            //Act 
            var result = await _customerControllerMock.RegisterNewCustomer(new DataTransferObjects.CustomerDTO
            {
                FirstName = string.Empty,
                LastName = testCustomer.LastName,
                CountryCode = testCountry.ISOCode,
                Email = testCustomer.Email
            });

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(BadRequestObjectResult)));
        }

        [Test]
        public async Task When_A_New_Customer_Wants_To_Be_Registered_On_The_System_And_Their_Email_Address_Is_Not_Associated_With_Another_Account_And_They_Have_All_Nessessary_Information_Except_Their_LastName_Is_Not_Provided_Then_A_Bad_Request_Response_Should_Be_Returned()
        {
            //Arrange
            testCustomer.Country = testCountry;
            testCustomer.CountryCodeId = testCountry.ID;

            _customerRepositoryMock.Setup(x => x.CustomerExistsWithEmailAsync(testCustomer.Email)).ReturnsAsync(false);
            _customerRepositoryMock.Setup(x => x.RegisterNewCustomerAsync(testCustomer.FirstName, testCustomer.LastName, testCustomer.Email, testCountry)).ReturnsAsync(true);

            _countryRepositoryMock.Setup(x => x.GetCountryByCountryCodeAsync(testCountry.ISOCode)).ReturnsAsync(testCountry);

            //Act 
            var result = await _customerControllerMock.RegisterNewCustomer(new DataTransferObjects.CustomerDTO
            {
                FirstName = testCustomer.FirstName,
                LastName = string.Empty,
                CountryCode = testCountry.ISOCode,
                Email = testCustomer.Email
            });

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(BadRequestObjectResult)));
        }

        [Test]
        public async Task When_A_New_Customer_Wants_To_Be_Registered_On_The_System_And_Their_Email_Address_Is_Not_Associated_With_Another_Account_And_They_Have_All_Nessessary_Information_Except_Their_CountryCode_Is_Not_Provided_Then_A_Bad_Request_Response_Should_Be_Returned()
        {
            //Arrange
            testCustomer.Country = testCountry;
            testCustomer.CountryCodeId = testCountry.ID;

            _customerRepositoryMock.Setup(x => x.CustomerExistsWithEmailAsync(testCustomer.Email)).ReturnsAsync(false);
            _customerRepositoryMock.Setup(x => x.RegisterNewCustomerAsync(testCustomer.FirstName, testCustomer.LastName, testCustomer.Email, testCountry)).ReturnsAsync(true);

            _countryRepositoryMock.Setup(x => x.GetCountryByCountryCodeAsync(testCountry.ISOCode)).ReturnsAsync(testCountry);

            //Act 
            var result = await _customerControllerMock.RegisterNewCustomer(new DataTransferObjects.CustomerDTO
            {
                FirstName = testCustomer.FirstName,
                LastName = testCustomer.LastName,
                CountryCode = string.Empty,
                Email = testCustomer.Email
            });

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(BadRequestObjectResult)));
        }

        [Test]
        public async Task When_A_New_Customer_Wants_To_Be_Registered_On_The_System_And_Their_Email_Address_Is_Not_Associated_With_Another_Account_And_They_Have_All_Nessessary_Information_Except_Their_Email_Is_Not_Provided_Then_A_Bad_Request_Response_Should_Be_Returned()
        {
            //Arrange
            testCustomer.Country = testCountry;
            testCustomer.CountryCodeId = testCountry.ID;

            _customerRepositoryMock.Setup(x => x.CustomerExistsWithEmailAsync(testCustomer.Email)).ReturnsAsync(false);
            _customerRepositoryMock.Setup(x => x.RegisterNewCustomerAsync(testCustomer.FirstName, testCustomer.LastName, testCustomer.Email, testCountry)).ReturnsAsync(true);

            _countryRepositoryMock.Setup(x => x.GetCountryByCountryCodeAsync(testCountry.ISOCode)).ReturnsAsync(testCountry);

            //Act 
            var result = await _customerControllerMock.RegisterNewCustomer(new DataTransferObjects.CustomerDTO
            {
                FirstName = testCustomer.FirstName,
                LastName = testCustomer.LastName,
                CountryCode = testCountry.ISOCode,
                Email = string.Empty
            });

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(BadRequestObjectResult)));
        }

        [Test]
        public async Task When_A_New_Customer_Wants_To_Be_Registered_On_The_System_And_They_Have_Provided_No_Information_Then_A_Bad_Request_Response_Should_Be_Returned()
        {
            //Arrange
            testCustomer.Country = testCountry;
            testCustomer.CountryCodeId = testCountry.ID;

            _customerRepositoryMock.Setup(x => x.CustomerExistsWithEmailAsync(testCustomer.Email)).ReturnsAsync(false);
            _customerRepositoryMock.Setup(x => x.RegisterNewCustomerAsync(testCustomer.FirstName, testCustomer.LastName, testCustomer.Email, testCountry)).ReturnsAsync(true);

            _countryRepositoryMock.Setup(x => x.GetCountryByCountryCodeAsync(testCountry.ISOCode)).ReturnsAsync(testCountry);

            //Act 
            var result = await _customerControllerMock.RegisterNewCustomer(null);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(BadRequestObjectResult)));
        }
    }
}
