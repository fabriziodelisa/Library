using Library.Models.DTOs;
using Library.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        readonly internal IBookServices _bookServices;
        public BookController(IBookServices bookService)
        {
            _bookServices = bookService;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var allBooks = _bookServices.GetAll();
            return Ok(allBooks);
        }

        [HttpGet]
        [Route("GetById/{id}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            try
            {
                var book = _bookServices.GetById(id);
                return Ok(book);
            }
            catch (KeyNotFoundException ex)
            {
               return NotFound(ex.Message);
            }           
        }

        [HttpGet]
        [Route("SearchByName")]
        public IActionResult SearhByName([FromQuery]string name)
        {
            try
            {
                var books = _bookServices.SearchByName(name);
                return Ok(books);
            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Add(CreateBookDTO book)
        {
            try
            {
                var addedBook = _bookServices.Add(book);

                return CreatedAtRoute(
                    routeName: "GetById",
                    routeValues: new { id = addedBook.Id },
                    value: addedBook
                );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Available")]
        public IActionResult Available(bool available, int id)
        {
            try
            {
                var result = _bookServices.Available(available, id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                var deletedBook = _bookServices.Delete(id);
                return Ok($"{deletedBook.Title} was delete successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
