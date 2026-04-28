const url_server = `http://localhost:5080/api`

document.addEventListener('DOMContentLoaded', function () {
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
    fetch(
        url_server + '/apiusers/register',
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                "UserName": username,
                "Email": email,
                "Password": password
            })
        }
    ).then(response => {
        if (response.ok) {
            alert("Register successfully ...")
        } else {
            throw new Error(`Status: ${response.status}, message: ${response.statusText}`)
        }
    }).catch(err => alert(err))
}

async function login() {
    const email = document.getElementById('input_login_email_id').value
    const password = document.getElementById('input_login_password_id').value

    fetch(url_server + '/apiusers/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            Email: email,
            Password: password
        })
    })
        .then(response => {
            if (!response.ok)
                throw new Error(`Error: ${response.status}`)
            return response.json()
        })
        .then(data => {
            console.log(data)

            // ✅ правильне збереження токена
            localStorage.setItem("token", data.token)
            //Додати адресу головної сторінки фронтенда
            //window.location.href = "/"
            const user = parseUserFromToken(data.token)
            /*
                {
                username: "admin@gmail.com",
                email: "admin@gmail.com",
                role: "admin"
                }
            */
            if (!user.username && !user.email && !user.role) {
                throw new Error('Login error ... [!username]')
                return
            }
            alert(`Hello: ${user.username}`)
            localStorage.setItem('username', user.username)
            localStorage.setItem('email', user.email)
            localStorage.setItem('role', user.role)
        })
        .catch(err => console.log(err))
}

function parseUserFromToken(token) {
    try {
        const payloadBase64 = token.split('.')[1];

        // декодуємо base64 (підтримка unicode)
        const payloadJson = decodeURIComponent(
            atob(payloadBase64)
                .split('')
                .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
                .join('')
        );

        const payload = JSON.parse(payloadJson);

        return {
            username: payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] || null,
            email: payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"] || null,
            role: payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || null
        };
    } catch (e) {
        console.error("Invalid token", e);
        return null;
    }
}