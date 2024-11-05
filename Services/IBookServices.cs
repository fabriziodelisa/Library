using Library.Models.DTOs;
using Library.Models.Entities;

namespace Library.Services
{
    public interface IBookServices
    {
        IEnumerable<BookDTO> GetAll();
        BookDTO Add(CreateBookDTO book);
        IEnumerable<BookDTO> SearchByName(string name);
        string Available(bool available, int id);
        BookDTO GetById(int id);
        BookDTO Delete(int id);
    }
}
