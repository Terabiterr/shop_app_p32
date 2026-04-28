document.addEventListener('DOMContentLoaded', function() {
    const profile_name = document.getElementById('profile_name')
    const profile_email = document.getElementById('profile_email')
    const profile_role = document.getElementById('profile_role')
    profile_name.textContent = localStorage.getItem('username')
    profile_email.textContent = localStorage.getItem('email')
    profile_role.textContent = localStorage.getItem('role')
})