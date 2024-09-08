using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using PayPalCheckoutSdk.Orders;
using Project7.DTOs;
using Project7.Models;
using System.IO;

namespace Project7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAdressController : ControllerBase
    {
        private readonly MyDbContext _context;

        public UserAdressController(MyDbContext context)
        {
            _context = context;
        }

        [Route("userAddresses/{userId}")]
        [HttpGet]
        public IActionResult ProductById(int userId)
        {
            var adresses = _context.UserAddresses.Where(c => c.UserId == userId).ToList();

            return Ok(adresses);
        }

        [Route("name/{id}")]
        [HttpGet]

        public IActionResult GetNameById(int id)
        {
            if (id < 0)
            {

                return BadRequest($"Invalid input: {id}");
            }

            var user = _context.Users.Where(model => model.UserId == id).ToList();

            if (user == null)
            {
                return NotFound($"User '{id}' not found.");
            }

            return Ok(user);
        }


        [HttpGet("{id}")]
        public IActionResult GetAllData(int id)
        {
            var getData = _context.UserAddresses
                .Where(x => x.User.UserId == id) // Filter by UserId
                .Select(x => new UserAdressResponseDTO
                {
                    AddressId = x.AddressId,
                    Street = x.Street,
                    City = x.City,
                    HomeNumberCode = x.HomeNumberCode,
                    User = new UserDTO
                    {
                        UserId = x.User.UserId,
                        FirstName = x.User.FirstName,
                        LastName = x.User.LastName,
                        Email = x.User.Email,
                        Phone = x.User.Phone,
                    }
                })
                .ToList();

            if (getData == null || !getData.Any())
            {
                return NotFound($"No addresses found for user with ID {id}");
            }

            return Ok(getData);
        }

    }
}
