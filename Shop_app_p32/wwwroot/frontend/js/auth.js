const url_server = `http://localhost:5080/api`

document.addEventListener('DOMContentLoaded', function() {
    const form_login = document.getElementById('form_login')
    const form_register = document.getElementById('form_register')
    form_login.style.display = 'none'
    form_register.style.display = 'none'
})

function show_form_register() {
    const form_login = document.getElementById('form_login')
    const form_register = document.getElementById('form_register')
    form_login.style.display = 'none'
    form_register.style.display = 'block'
}

function show_form_login() {
    const form_login = document.getElementById('form_login')
    const form_register = document.getElementById('form_register')
    form_login.style.display = 'block'
    form_register.style.display = 'none'
}

function register() {
    const username = document.getElementById('input_register_username_id').value
    const email = document.getElementById('input_register_email_id').value
    const password = document.getElementById('input_register_password_id').value
    console.log(`Username: ${username}, E-mail: ${email}, Password: ${password}`)
}

function login() {

}

async function getToken(username, email, password) {
    const url_auth = `http://localhost:5080/api/apiusers/login`
    return await fetch(
        url_auth,
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                Username: username,
                email: email,
                PasswordHash: password
            })
        }
    ).then(response => {
        if(!response.ok)
            throw new Error('Fail to fetch JWT Token ...')
        return response.json()
    }).then(data => {
        return data.token.result
    })
    .catch(err => console.log(err))
}