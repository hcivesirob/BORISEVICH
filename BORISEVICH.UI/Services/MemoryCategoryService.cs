using BORISEVICH.Domain.Entities;
using BORISEVICH.Domain.Models;

namespace BORISEVICH.UI.Services
{
    public class MemoryCategoryService:ICategoryService
    {
        public Task<ResponseData<List<Category>>>
        GetCategoryListAsync()
        {
            var categories = new List<Category>
            {
                new Category {Id=1, Name="Повесть",
                NormalizedName="story"},
                new Category {Id=2, Name="Роман",
                NormalizedName="novel"},
                new Category {Id=3, Name="Детектив",
                NormalizedName="detective"}
            };
            var result = new ResponseData<List<Category>>();
            result.Data = categories;
            return Task.FromResult(result);
        }
    }
}
