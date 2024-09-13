async function getCategory() {
    const url = "https://localhost:44325/api/Categories/GetAllCategories";
    let response = await fetch(url);
    if (!response.ok) {
        console.error("Failed to fetch categories");
        return;
    }
    var result = await response.json();
    const dropdown = document.getElementById('dynamicDropdown');

    // Populate the dropdown with options
    result.forEach(category => {
        const option = document.createElement('option');
        option.value = category.categoryId;
        option.innerHTML = category.categoryName;
        dropdown.appendChild(option);
    });

    // Add event listener to store selected categoryId in localStorage
    dropdown.addEventListener('change', function () {

        const selectedCategoryId = dropdown.value;
        localStorage.setItem('selectedCategoryId', selectedCategoryId);
        console.log(`Category ID ${selectedCategoryId} saved to localStorage`);

        // Instead of reloading, call the function directly
        if (localStorage.getItem('selectedCategoryId') === 	"All Category") {
            clearStorage()
        }else {
            getProductsByCategoryId();
            }
    });
}

// Function to populate the product grid
function renderProduct(element, container) {
    container.innerHTML += `
        <li class="st-col-item">
            <div class="single-product-wrap">
                <!-- product-img start -->
                <div class="product-image">
                    <div  class="pro-img">
                        <img src="${element.productImage}" class="img-fluid img1" alt="p-1">
                    </div>
                    <div class="product-action" >
                        <a href="product-details.html" class="quick-view" onclick="saveProductId(${element.productId})" >
                            <span class="tooltip-text">Product Details</span>
                            <span class="quickview-icon"><i class="feather-eye"></i></span>                            
                        </a>
                        <a onclick="saveProductIdCART(${element.productId})" class="add-to-cart">
                    <span class="tooltip-text">Add to cart</span>
                    <span class="cart-icon"><i class="feather-shopping-bag"></i></span>
                </a>
                    </div>
                </div>
                <div class="product-content">
                    <h6>${element.productName}</h6>
                    <div class="price-box">
                        <span class="new-price">${element.price} JOD</span>
                    </div>
                </div>
            </div>
        </li>
    `;
}

async function saveProductIdCART(id)
{
 localStorage.setItem("productId",id);
    let n = localStorage.getItem("productId");
    let cartID = localStorage.getItem("CARTID") || Date.now();  // generate cartID if not present
    let cartItems = JSON.parse(localStorage.getItem("cartItems")) || [];
    console.log("befor cartItems", cartItems);
   
    let quantity = 1;
    let cartItemId = localStorage.getItem("CartItemId") ? parseInt(localStorage.getItem("CartItemId")) : 1;
    let response = await fetch(`https://localhost:44325/api/Products/${n}`);
    let result = await response.json();
    // console.log("result", result.price)
   

    let existingProductIndex = cartItems.findIndex(item => item.productID == n);
    if (existingProductIndex !== -1) {
           // Update quantity if the product is already in the cart
         cartItems[existingProductIndex].quantity += parseInt(quantity);
        }
        else {

    // Add new item to the cart with a unique CartItemId
    let product = {
        cartItemId: cartItemId,  // Set the auto-incremented CartItemId
        cartId: cartID,
        productID: n,
        quantity: parseInt(quantity),
        price: result.price,  // Example price, this should come from the actual product data
        imageUrl: result.productImage,  // Example image URL, replace with actual data
        name: result.productName  // Example product name, replace with actual data
    };

    cartItems.push(product);

    console.log("cartItems", cartItems);
    // Increment the CartItemId for the next item
    localStorage.setItem("CartItemId", cartItemId + 1);
}

if (localStorage.UserID == null){

    localStorage.setItem("cartItems", JSON.stringify(cartItems));
    window.alert("The item was added to the mini cart successfully.");
    updateMiniCart();  // Update the mini cart display
    event.preventDefault();
}
    else {
        var NewQuantity = 1;
        var id = localStorage.getItem("productId");

        let url = "https://localhost:44325/api/CartItem/addtocart";

        var request = {
            productId: id,
            quantity: NewQuantity,
            cartId: localStorage.CARTID,
        };

        let response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request),
        });

        window.location.href = "cart-page.html";
    }
}



function updateMiniCart() {
    let cartItems = JSON.parse(localStorage.getItem("cartItems")) || [];
    let cartContainer = document.querySelector('.cart-item');
    cartContainer.innerHTML = '';  // Clear existing items


  
    let subtotalValue = 0;

    let count=document.getElementById("count");
    let itemCount = cartItems.length;
count.innerHTML=` <span class="cart-count-desc">There are</span>
                    <span class="cart-count">${itemCount}</span>
                    <span class="cart-count-desc">products</span>`;
    if (cartItems.length === 0) {
        document.querySelector('.empty-cart').classList.remove('d-none');
        document.querySelector('.cart-text').classList.add('d-none');
    } else {
        document.querySelector('.empty-cart').classList.add('d-none');
        document.querySelector('.cart-text').classList.remove('d-none');
        cartItems.forEach(item => {
            cartContainer.innerHTML += `
                <li class="cart-product">
                    <div class="cart-img">
                        <a href="product-template.html" class="img-area">
                            <img src="${item.imageUrl}" class="img-fluid" alt="${item.name}">
                        </a>
                    </div>
                    <div class="cart-content">
                        <h6><a href="product-template.html">${item.name}</a></h6>
                        <div class="product-info">
                            <div class="info-item">
                                <span class="product-qty">${item.quantity}</span>
                                <span>×</span>
                                <span class="product-price">$${item.price}  </span>
                            </div>
                        </div>
                        <div class="product-quantity-action">
                            <button class="delete-icon" onclick="removeCartItem(${item.cartItemId})">
                                <i class="feather-trash-2"></i>
                            </button>
                        </div>
                    </div>
                </li>
            `;
            
            subtotalValue += item.price * item.quantity;
        });
        let subtotal = document.getElementById("subtotal");
    subtotal.innerHTML = `<span class="subtotal-price">€${subtotalValue.toFixed(2)}</span>`;
    }
}

// Call this function to load the cart on page load
updateMiniCart();

function removeCartItem(cartItemId) {
    let cartItems = JSON.parse(localStorage.getItem("cartItems")) || [];
    let updatedCartItems = cartItems.filter(item => item.cartItemId != cartItemId);

    localStorage.setItem("cartItems", JSON.stringify(updatedCartItems));
    updateMiniCart();  // Update the display after deletion

    window.alert("Item removed from cart.");
}


async function checkoutCart() {
    if(localStorage.UserID==null){
        window.location.href="login-account.html";
    }
    let cartItems = JSON.parse(localStorage.getItem("cartItems")) || [];
    if (cartItems.length === 0) {
        window.alert("Your cart is empty!");
        return;
    }

    const url = "https://localhost:44325/api/CartItem/addtocart";  // Your backend endpoint for saving cart items

    for (let item of cartItems) {
        let response = await fetch(url, {
            method: 'POST',
            body: JSON.stringify(item),
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            window.alert("Please login first");
            return;
        }
    }

    window.alert("Checkout completed successfully!");
    localStorage.removeItem("cartItems");  // Clear the cart after checkout
    location.reload();  // Reload the page or redirect to a success page
}
















// Fetch and display products by category
async function getProductsByCategoryId() {
    const selectedCategoryId = localStorage.getItem("selectedCategoryId");
    if (!selectedCategoryId) return;
    
    const url = `https://localhost:44325/api/Products/category/${selectedCategoryId}`;
    let response = await fetch(url);
    if (!response.ok) {
        console.error("Failed to fetch products for category");
        return;
    }
    const result = await response.json();
    const container = document.getElementById("product-grid");
    container.innerHTML = ''; // Clear existing products

    result.forEach(element => renderProduct(element, container));
}

// Fetch and display all products
async function getAllProduct() {
    const url = "https://localhost:44325/api/Products";
    let response = await fetch(url);
    if (!response.ok) {
        console.error("Failed to fetch all products");
        return;
    }
    const result = await response.json();
    const container = document.getElementById("product-grid");
    container.innerHTML = ''; // Clear existing products

    result.forEach(element => renderProduct(element, container));
}

// Call the appropriate function based on selected category
if (!localStorage.getItem("selectedCategoryId")) {
    getAllProduct();
} else {
    getProductsByCategoryId();
}

// Clear localStorage function
function clearStorage() {
    debugger
    localStorage.removeItem("selectedCategoryId");
    getAllProduct();
}

// Call the function to populate the dropdown on page load
getCategory();





function updatePriceRange() {

    const minPrice = document.getElementById('range1').value;
    const maxPrice = document.getElementById('range2').value;

    // Debugging log
    console.log("Min Price:", minPrice);
    console.log("Max Price:", maxPrice);

    // document.getElementById('demo1').innerHTML = minPrice;
    // document.getElementById('demo2').innerHTML = maxPrice;

    fetchProductsByPriceRange(minPrice, maxPrice);
}


async function fetchProductsByPriceRange(minPrice, maxPrice) {
    const url = `https://localhost:44325/api/Products/priceRange?minPrice=${minPrice}&maxPrice=${maxPrice}`;

    try {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error('No products found within the specified price range.');
        }
        
        const products = await response.json();
        displayFilteredProducts(products);
    } catch (error) {
        console.error('Error fetching products:', error);
    }
}

function displayFilteredProducts(products) {
    const container = document.getElementById("product-grid");
    container.innerHTML = "";

    products.forEach(element => {
        container.innerHTML +=  `
        <li class="st-col-item">
            <div class="single-product-wrap">
                <!-- product-img start -->
                <div class="product-image">
                    <div  class="pro-img">
                        <img src="${element.productImage}" class="img-fluid img1" alt="p-1">
                    </div>
                    <div class="product-action" >
                        <a href="product-details.html" class="quick-view" onclick="saveProductId(${element.productId})" >
                            <span class="tooltip-text">Product Details</span>
                            <span class="quickview-icon"><i class="feather-eye"></i></span>                            
                        </a>
                        <a onclick="saveProductIdCART(${element.productId})" class="add-to-cart">
                    <span class="tooltip-text">Add to cart</span>
                    <span class="cart-icon"><i class="feather-shopping-bag"></i></span>
                </a>
                    </div>
                </div>
                <div class="product-content">
                    <h6>${element.productName}</h6>
                    <div class="price-box">
                        <span class="new-price">${element.price} JOD</span>
                    </div>
                </div>
            </div>
        </li>
    `;
    });
}

function saveProductId(productId) {
    localStorage.setItem('productId', productId);
}

////////////////
///Searching for products
//////////////////////

function searchProducts() {
    debugger
    const searchQuery = document.getElementById('productSearchInput').value;

    // Fetch products filtered by search query
    fetchProductsByName(searchQuery);
}

async function fetchProductsByName(query) {
    const url = `https://localhost:44325/api/Products/search?query=${encodeURIComponent(query)}`;

    try {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error('No products found with the specified search term.');
        }
        const products = await response.json();
        console.log('API Products:', products); // Log API products for debugging
        displayFilteredProducts(products);
    } catch (error) {
        console.error('Error fetching products:', error);
    }
}

