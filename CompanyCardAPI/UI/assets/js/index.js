const API_BASE = 'http://localhost:5000/api/companies'; // Adjust if hosted elsewhere

// Registration
document.getElementById('register-form').addEventListener('submit', async (e) => {
  e.preventDefault();
  const email = document.getElementById('register-email').value;

  try {
    const res = await fetch(`${API_BASE}/register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email })
    });

    const data = await res.json();
    alert(data.message || 'OTP sent to email!');
  } catch (err) {
    console.error(err);
    alert('Error during registration.');
  }
});

// OTP Verification
document.getElementById('verify-form').addEventListener('submit', async (e) => {
  e.preventDefault();
  const email = document.getElementById('verify-email').value;
  const otp = document.getElementById('verify-otp').value;
  const password = document.getElementById('verify-password').value;

  try {
    const res = await fetch(`${API_BASE}/verify`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, otp, password })
    });

    const data = await res.json();
    if (res.ok) {
      alert('Verification successful. You can now log in.');
    } else {
      alert(data.message || 'Verification failed.');
    }
  } catch (err) {
    console.error(err);
    alert('Error during verification.');
  }
});

// Login
document.getElementById('login-form').addEventListener('submit', async (e) => {
  e.preventDefault();
  const email = document.getElementById('login-email').value;
  const password = document.getElementById('login-password').value;

  try {
    const res = await fetch(`${API_BASE}/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password })
    });

    const data = await res.json();
    if (res.ok) {
      localStorage.setItem('token', data.token);
      window.location.href = 'dashboard.html';
    } else {
      alert(data.message || 'Login failed.');
    }
  } catch (err) {
    console.error(err);
    alert('Error during login.');
  }
});
