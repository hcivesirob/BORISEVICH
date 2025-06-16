using BORISEVICH.Domain.Entities;
using BORISEVICH.Domain.Models;

namespace BORISEVICH.UI.Services
{
    public interface ICategoryService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
