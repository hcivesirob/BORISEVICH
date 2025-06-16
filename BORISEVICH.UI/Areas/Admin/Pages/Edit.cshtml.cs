using BORISEVICH.Domain.Entities;
using BORISEVICH.UI.Data;
using BORISEVICH.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BORISEVICH.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public EditModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;
        public SelectList Categories { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookResponse = await _productService.GetProductByIdAsync(id.Value);
            if (bookResponse == null)
            {
                return NotFound();
            }
            Book = bookResponse.Data;
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            Categories = new SelectList(categoriesResponse.Data, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Если модель невалидна, загружаем категории снова
                var categoriesResponse = await _categoryService.GetCategoryListAsync();
                Categories = new SelectList(categoriesResponse.Data, "Id", "Name");
                return Page();
            }

            // Обновляем книгу через сервис
            var updateResponse = await _productService.UpdateProductAsync(Book.Id, Book, null);
            if (!updateResponse.Success)
            {
                ModelState.AddModelError(string.Empty, "Ошибка при обновлении книги");
                var categoriesResponse = await _categoryService.GetCategoryListAsync();
                Categories = new SelectList(categoriesResponse.Data, "Id", "Name");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
