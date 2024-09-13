using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project7Candy.DTO;
using Project7Candy.Models;

namespace Project7Candy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly MyDbContext _db;
        public CartItemController(MyDbContext db)
        {
            _db = db;
        }
        [HttpGet("cartitem")]
        public IActionResult getCartItems()
        {
            var cartItem = _db.CartItems.Select(x => new cartItemResponseDTO
            {
                CartId = x.CartId,
                CartItemId = x.CartItemId,
                Quantity = x.Quantity,
                Product = new ProductMainRequest
                {
                    ProductId = x.Product.ProductId,
                    ProductName = x.Product.ProductName,
                    Price = x.Product.Price,
                }
            });
            return Ok(cartItem);
        }

        [HttpPost("addtocart")]
        public IActionResult AddCartItems([FromBody] CartItemBatchRequest cartBatch)
        {
            var productIds = cartBatch.Items.Select(i => i.ProductId).ToList();
            var products = _db.Products.Where(p => productIds.Contains(p.ProductId)).ToList();

            if (products.Count != cartBatch.Items.Count)
            {
                return NotFound("Some products do not exist.");
            }

            foreach (var item in cartBatch.Items)
            {
                var product = products.SingleOrDefault(p => p.ProductId == item.ProductId);

                // Step 2: Check if the requested quantity is available
                if (product.Stock < item.Quantity)
                {
                    return BadRequest($"The requested quantity for product {product.ProductName} is not available.");
                }

                // Step 3: Check if the product already exists in the user's cart
                var existingCartItem = _db.CartItems.SingleOrDefault(ci => ci.CartId == cartBatch.CartId && ci.ProductId == item.ProductId);

                if (existingCartItem != null)
                {
                    // If the product exists in the cart, just update the quantity
                    existingCartItem.Quantity += item.Quantity;
                }
                else
                {
                    // If the product does not exist, create a new cart item
                    var cartItem = new CartItem
                    {
                        CartId = cartBatch.CartId,
                        Quantity = item.Quantity,
                        ProductId = item.ProductId
                    };

                    _db.CartItems.Add(cartItem);
                }

                // Step 4: Update the product's stock in the database
                product.Stock -= item.Quantity;
            }

            // Step 5: Save the changes to the database
            _db.SaveChanges();

            return Ok();
        }

        // Define the request body
        public class CartItemBatchRequest
        {
            public int CartId { get; set; }
            public List<CartItemRequest> Items { get; set; }
        }






        [HttpPut("update/updateCart/{id}")]
        public IActionResult updateCart(int id, [FromBody] CartItemRequest cart)
        {
            var cartItem = _db.CartItems.FirstOrDefault(ci => ci.CartItemId == id);

            if (cartItem == null)
            {
                return BadRequest();
            }
            cartItem.Quantity = cart.Quantity;
            var updatecItems = _db.CartItems.Update(cartItem);
            _db.SaveChanges();
            if (updatecItems == null)
            {
                return BadRequest();
            }
            return Ok(updatecItems);
        }

        [HttpDelete("delete/deleteItem/{id}")]
        public IActionResult deleteItem(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var cartItem = _db.CartItems.FirstOrDefault(ci => ci.CartItemId == id);

            if (cartItem == null)
            {
                return NotFound();
            }
            _db.CartItems.Remove(cartItem);
            _db.SaveChanges();
            return NoContent();
        }

        //[HttpGet("cart/GetCartItemsForUser/{userID}")]
        //public IActionResult GetCartItemsForUser(int userID)
        //{
        //    var user = _db.Carts.FirstOrDefault(x => x.UserId == userID);
        //    var cartItem = _db.CartItems.Where(ci => ci.CartId == user.CartId).Select(
        //        x => new cartItemResponseDTO
        //        {
        //            CartItemId = x.CartItemId,
        //            CartId = x.CartId,
        //            Quantity = x.Quantity,
        //            Product = new ProductMainRequest
        //            {
        //                ProductId = x.Product.ProductId,
        //                ProductName = x.Product.ProductName,
        //                Price = x.Product.Price,
        //                ProductImage = x.Product.ProductImage,
        //            },
        //            Category = new CategoryMainRequest
        //            {
        //                CategoryName=x.Category.CategoryName,
        //            }
        //        });
        //    return Ok(cartItem);
        //}

        //[HttpGet("cart/GetCartItemsForProduct/{Productid}")]
        //public IActionResult GetCartItemsForProduct(int Productid)
        //{
        //    var user = _db.CartItems.FirstOrDefault(x => x.UserId == userID);
        //    var cartItem = _db.CartItems.Where(ci => ci.CartId == user.CartId).Select(
        //    return Ok(); 
        //}

        [HttpGet("cartitem/{id}")]
        public IActionResult getCartItems(int id)
        {
            var cartItem = _db.CartItems.Where(c => c.ProductId == id).ToList();
            return Ok(cartItem);
        }
        ///////////////////////////////////////////////////////////////////////////////////

        [Authorize]
        [HttpGet("GetCartItems/{userID}")]
        public IActionResult GetCartItems(int userID)
        {
            var cartItems = _db.CartItems.Select(p => new
            {
                p.Quantity,
                p.CartItemId,
                Cart = new
                {
                    p.Cart.UserId,
                },
                Product = new
                {
                    p.Product.ProductImage,
                    p.Product.ProductName,
                    p.Product.Price,
                    p.Product.Category.CategoryName,
                }
            }).Where(p => p.Cart.UserId == userID).ToList();
            return Ok(cartItems);
        }



        [HttpGet("cart/GetCartItemsForUser/{userID}")]
        public IActionResult GetCartItemsForUser(int userID)
        {
            var user = _db.Carts.FirstOrDefault(x => x.UserId == userID);
            var cartItem = _db.CartItems.Where(ci => ci.CartId == user.CartId).Select(
                x => new cartItemResponseDTO
                {
                    CartItemId = x.CartItemId,
                    CartId = x.CartId,
                    Quantity = x.Quantity,
                });
            return Ok(cartItem);
        }

        [HttpGet("getCartbyUserIDDDD/{id}")]
        public IActionResult getUsersandCarts(int id)
        {
            var cartUser = _db.Carts.FirstOrDefault(p => p.UserId == id);
            return Ok(cartUser);
        }

        [HttpPost("createCart/{userID}")]
        public IActionResult createCart([FromForm] int userID, CreateCartDTO cart)
        {
            var x = new Cart
            {
                UserId = cart.UserId,
            };
            _db.Carts.Add(x);
            _db.SaveChanges();
            return Ok(cart);
        }

    }
}
