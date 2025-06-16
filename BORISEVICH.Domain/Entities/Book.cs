using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BORISEVICH.Domain.Entities
{
    public class Book:Entity
    {
        public int Id { get; set; } // id книги
        public string Name { get; set; } // название книги
        public string Author { get; set; } // автор книги
        public int Page { get; set; } // кол. страниц
        public string? Image { get; set; } // путь к файлу изображения
                                           // Навигационные свойства
        /// <summary>
        /// группа книг по автору
        /// </summary>
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
