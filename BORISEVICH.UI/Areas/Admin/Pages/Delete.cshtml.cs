using BORISEVICH.Domain.Entities;
using BORISEVICH.UI.Data;
using BORISEVICH.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BORISEVICH.UI.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IProductService _productService;

        public DeleteModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _productService.GetProductByIdAsync(id.Value);

            if (response.Data == null)
            {
                return NotFound();
            }
            else
            {
                Book = response.Data;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var deleteResponse = await _productService.DeleteProductAsync(id.Value);

            if (!deleteResponse.Success)
            {
                ModelState.AddModelError(string.Empty, "Ошибка при удалении книги");
                return await OnGetAsync(id); // Повторно загружаем данные
            }

            return RedirectToPage("./Index");
        }
    }
}
