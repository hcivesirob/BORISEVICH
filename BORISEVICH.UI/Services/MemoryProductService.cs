using BORISEVICH.Domain.Entities;
using BORISEVICH.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BORISEVICH.UI.Services
{
    public class MemoryProductService : IProductService
    {
        private readonly ICategoryService _categoryService;
        private readonly IConfiguration _config;
        List<Book> _books;
        List<Category> _categories;
        public MemoryProductService([FromServices] IConfiguration config, ICategoryService categoryService)
        {
            _categories = categoryService.GetCategoryListAsync().Result.Data;
            _config = config;
            SetupData();
        }

        /// <summary>
        /// Инициализация списков
        /// </summary>
        private void SetupData()
        {
            _books = new List<Book>
                {
                    new Book {Id = 1, Name="Белые ночи",
                        Author="Достоевский Ф.М.",
                        Page =320, Image="Images/dostoevski.jpg",
                        CategoryId=_categories.Find(c=>c.NormalizedName.Equals("story")).Id},
                    new Book { Id = 2, Name="Морфий",
                        Author="Булгаков М.А.",
                        Page =330, Image="Images/bulgakov.jpg",
                        CategoryId=_categories.Find(c=>c.NormalizedName.Equals("novel")).Id},
                    new Book {Id = 3, Name="Атлант расправил плечи",
                        Author="Рэнд А.",
                        Page =1136, Image="Images/rand.jpg",
                        CategoryId=_categories.Find(c=>c.NormalizedName.Equals("novel")).Id},
                    new Book {Id = 4, Name="Каласы под сярпом тваiм",
                        Author="Короткевич В.С.",
                        Page =928, Image="Images/karatkevich.jpg",
                        CategoryId=_categories.Find(c=>c.NormalizedName.Equals("novel")).Id},
                    new Book {Id = 5, Name="Токийский зодиак",
                        Author="Симада С.",
                        Page =352, Image="Images/simada.jpg",
                        CategoryId=_categories.Find(c=>c.NormalizedName.Equals("detective")).Id},
                };
        }
        public Task<ResponseData<ProductListModel<Book>>>
            GetProductListAsync(
                        string? categoryNormalizedName,
                        int pageNo = 1)
        {
            // Создать объект результата
            var result = new ResponseData<ProductListModel<Book>>();
            // Id категории для фильрации
            int? categoryId = null;

            // если требуется фильтрация, то найти Id категории
            // с заданным categoryNormalizedName
            if (categoryNormalizedName != null)
                categoryId = _categories
                .Find(c =>
                c.NormalizedName.Equals(categoryNormalizedName))
                ?.Id;

            // Выбрать объекты, отфильтрованные по Id категории,
            // если этот Id имеется
            var data = _books
            .Where(d => categoryId == null ||
            d.CategoryId.Equals(categoryId))?
            .ToList();

            // получить размер страницы из конфигурации
            int pageSize = _config.GetSection("ItemsPerPage").Get<int>();

            // получить общее количество страниц
            int totalPages = (int)Math.Ceiling(data.Count /
            (double)pageSize);
            // получить данные страницы
            var listData = new ProductListModel<Book>()
            {
                Items = data.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            // поместить данные в объект результата
            result.Data = listData;

            // Если список пустой
            if (data.Count == 0)
            {
                result.Success = false;
                result.ErrorMessage = "Нет объектов в выбраннной категории";
            }
            // Вернуть результат
            return Task.FromResult(result);
        }

        public Task<ResponseData<Book>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductAsync(int id, Book product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Book>> CreateProductAsync(Book product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        Task<ResponseData<Book>> IProductService.UpdateProductAsync(int id, Book product, IFormFile formFile)
        {
            throw new NotImplementedException();
        }

        Task<ResponseData<bool>> IProductService.DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
