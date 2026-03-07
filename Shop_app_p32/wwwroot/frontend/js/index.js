const url_server = `http://localhost:5080/api`
document.addEventListener('DOMContentLoaded', function () {
    let username = localStorage.getItem('username')
    if (username === null) {
        document.getElementById("register").style.display = 'block'
        document.getElementById("login").style.display = 'block'
    }
    else {
        document.getElementById("register").style.display = 'none'
        document.getElementById("login").style.display = 'none'
        document.getElementById("cart").style.display = 'block'
        document.getElementById("profile").style.display = 'block'
    }
})
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
        //Errors!!!
        const div_products = document.getElementById("products")
        data.forEach(product => {
            console.log(product)
        });
    }).catch(err => console.log(err))
}
get_products()