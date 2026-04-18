const url_server = `http://localhost:5080/api`
document.addEventListener('DOMContentLoaded', function () {
    let username = localStorage.getItem('username')
    if (username === null) {
        document.getElementById("auth").style.display = 'block'
    }
    else {
        document.getElementById("auth").style.display = 'none'
        document.getElementById("cart").style.display = 'block'
        document.getElementById("profile").style.display = 'block'
    }
})

function auth_location() {
    window.open("html/auth.html")
}

async function get_products() {
    await fetch(
        url_server + '/apiproducts',
        {
            metthod: 'GET',
            headers: {
                "Content-Type": "application/json"
            }
        }
    ).then(res => {
        return res.json()
    }).then(data => {
        const div_products = document.getElementById("products")
        data.value.forEach(product => {
        let card = `
            <div class="card" style="width: 18rem;">
            <img src="./img/${product.productImages[0].imageUrl}" class="card-img-top" alt="...">
            <div class="card-body">
                <h5 class="card-title">${product.name}</h5>
                <h5 class="card-title">${product.price}</h5>
                <h5 class="card-title">${product.quantity}</h5>
                <p class="card-text">${product.description}</p>
                <button class="btn_buy" onclick="add_to_cart(${product.id})">buy</button>
            </div>
            </div>
        `
            div_products.innerHTML += card
        });
    }).catch(err => console.log(err))
}
async function add_to_cart(productId) {
    alert(productId)
}
get_products()
