const url_server = `http://localhost:5080/api`

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