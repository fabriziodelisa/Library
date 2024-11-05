using AutoMapper;
using Library.Models.DTOs;
using Library.Models.Entities;
using Library.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class BookServices : IBookServices
    {
        internal readonly IBookRepository _bookRepository;
        internal readonly IMapper _mapper;
        public BookServices( IBookRepository booksRepository, IMapper mapper)
        {
            _bookRepository = booksRepository;
            _mapper = mapper;
        }

        public IEnumerable<BookDTO> GetAll()
        {
            var allBooks = _bookRepository.GetAll();
            return _mapper.Map<IEnumerable<BookDTO>>(allBooks);
        }

        public BookDTO GetById(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                throw new KeyNotFoundException($"Book with ID {id} was not found");
            }
            return  _mapper.Map<BookDTO>(book);
        }

        public IEnumerable<BookDTO> SearchByName(string name)
        {
            var books = _bookRepository.SearchByName(name);
            if(!books.Any())
            {
                throw new Exception($"There is no book with the author or title '{name}'");
            }

            return _mapper.Map<IEnumerable<BookDTO>>(books);
        }
        public BookDTO Add(CreateBookDTO book)
        {
            try
            {
                if (book == null)
                    throw new ArgumentNullException(nameof(book));

                var newBook = _mapper.Map<Book>(book);
                newBook.RegistrationDate = DateTime.UtcNow;
                newBook.Available = true;

                var addedBook = _bookRepository.Add(newBook);

                return _mapper.Map<BookDTO>(addedBook);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error saving to database", ex);
            }
        }

        public string Available(bool available, int id)
        {
            try
            {
                bool isAvailable = _bookRepository.Available(available, id);
                return $"The book whit id {id} is now {(isAvailable ? "Available" : "Unavailable")}";
            }
            catch (KeyNotFoundException ex)
            {
                throw;
            }
        }

        public BookDTO Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id must be greater than 0");
            try
            {
                var deleteBook = _bookRepository.Delete(id);
                return _mapper.Map<BookDTO>(deleteBook);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting book with id {id}", ex);
            }
        }
    }
}
