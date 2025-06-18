using BORISEVICH.API.Controllers;
using BORISEVICH.API.Data;
using BORISEVICH.Domain.Entities;
using BORISEVICH.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORISEVICH.Tests
{
    public class BookApiControllerTests : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<ProductDbContext> _contextOptions;
        private readonly IWebHostEnvironment _environment;
        public BookApiControllerTests()
        {
            _environment = Substitute.For<IWebHostEnvironment>();
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<ProductDbContext>()
            .UseSqlite(_connection)
            .Options;

            // Create the schema and seed some data
            using var context = new ProductDbContext(_contextOptions);
            context.Database.EnsureCreated();
            var categories = new Category[]
            {
                new Category {Name="Повесть", NormalizedName="story"},
                new Category {Name="Роман", NormalizedName="novel"},
                new Category {Name="Детектив", NormalizedName="detective"}
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();
            var books = new List<Book>
                {
                    new Book {Name="Белые ночи", Author="Достоевский Ф.М.", Page=320,
                    Category=categories
                    .FirstOrDefault(c=>c.NormalizedName.Equals("story"))},
                    new Book {Name="Морфий", Author="Булгаков М.А.", Page=330,
                    Category=categories
                    .FirstOrDefault(c=>c.NormalizedName.Equals("novel"))},
                    new Book {Name="Атлант расправил плечи", Author="Рэнд А.", Page=1136,
                    Category=categories
                    .FirstOrDefault(c=>c.NormalizedName.Equals("novel"))},
                    new Book {Name="Каласы под сярпом тваiм", Author="Короткевич В.С.", Page=928,
                    Category=categories
                    .FirstOrDefault(c=>c.NormalizedName.Equals("novel"))},
                    new Book {Name="Токийский зодиак", Author="Симада С.", Page=320,
                    Category=categories
                    .FirstOrDefault(c=>c.NormalizedName.Equals("detective"))},
                };
            context.AddRange(books);
            context.SaveChanges();
        }
        public void Dispose() => _connection?.Dispose();
        ProductDbContext CreateContext() => new ProductDbContext(_contextOptions);
        // Проверка фильтра по категории
        [Fact]
        public async void ControllerFiltersCategory()
        {
            // arrange
            using var context = CreateContext();
            var category = context.Categories.First();
            var controller = new BooksController(context, _environment);
            // act
            var response = await controller.GetBook(category.NormalizedName);
            ResponseData<ProductListModel<Book>> responseData = response.Value;
            var booksList = responseData.Data.Items; // полученный список объектов
                                                      //assert
            Assert.True(booksList.All(d => d.CategoryId == category.Id));
        }
        // Проверка подсчета количества страниц
        // Первый параметр - размер страницы
        // Второй параметр - ожидаемое количество страниц (при условии, что всего объектов 5)
        [Theory]
        [InlineData(2, 3)]
        [InlineData(3, 2)]
        public async void ControllerReturnsCorrectPagesCount(int size, int qty)
        {
            using var context = CreateContext();
            var controller = new BooksController(context, _environment);
            // act
            var response = await controller.GetBook(null, 1, size);
            ResponseData<ProductListModel<Book>> responseData = response.Value;
            var totalPages = responseData.Data.TotalPages; // полученное количество страниц
            //assert
            Assert.Equal(qty, totalPages); // количество страниц совпадает
        }
        [Fact]
        public async void ControllerReturnsCorrectPage()
        {
            using var context = CreateContext();
            var controller = new BooksController(context, _environment);
            // При размере страницы 3 и общем количестве объектов 5
            // на 2-й странице должно быть 2 объекта
            int itemsInPage = 2;
            // Первый объект на второй странице
            Book firstItem = context.Books.ToArray()[3];
            // act
            // Получить данные 2-й страницы
            var response = await controller.GetBook(null, 2);
            ResponseData<ProductListModel<Book>> responseData = response.Value;
            var booksList = responseData.Data.Items; // полученный список объектов
            var currentPage = responseData.Data.CurrentPage; // полученный номер текущей страницы
            //assert
            Assert.Equal(2, currentPage);// номер страницы совпадает
            Assert.Equal(2, booksList.Count); // количество объектов на странице равно 2
            Assert.Equal(firstItem.Id, booksList[0].Id); // 1-й объект в списке правильный
        }
    }
}
