const API_BASE = 'http://localhost:5106/swagger/index.html'; // Adjust if hosted elsewhere

const token = localStorage.getItem('token');
if (!token) {
  alert('Unauthorized. Please log in.');
  window.location.href = 'index.html';
}

// Fetch and render cards on load
window.addEventListener('DOMContentLoaded', fetchCards);

// Create card
document.getElementById('generate-card-form').addEventListener('submit', async (e) => {
  e.preventDefault();
  const cardNumber = document.getElementById('card-number').value;
  const pin = document.getElementById('card-pin').value;

  try {
    const res = await fetch(API_BASE, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`
      },
      body: JSON.stringify({ cardNumber, pin })
    });

    const data = await res.json();
    if (res.ok) {
      alert('Card created.');
      document.getElementById('generate-card-form').reset();
      fetchCards(); // Refresh table
    } else {
      alert(data.message || 'Failed to create card.');
    }
  } catch (err) {
    console.error(err);
    alert('Error creating card.');
  }
});

// Fetch cards
async function fetchCards() {
  try {
    const res = await fetch(API_BASE, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });

    const data = await res.json();
    const tbody = document.getElementById('cards-table-body');
    tbody.innerHTML = '';

    if (res.ok && Array.isArray(data)) {
      data.forEach((card, idx) => {
        const row = document.createElement('tr');
        row.innerHTML = `
          <th>${idx + 1}</th>
          <td>${card.cardNumber}</td>
          <td>${card.pin}</td>
        `;
        tbody.appendChild(row);
      });
    } else {
      alert(data.message || 'Failed to fetch cards.');
    }
  } catch (err) {
    console.error(err);
    alert('Error fetching cards.');
  }
}

// Logout
document.getElementById('logout-btn').addEventListener('click', () => {
  localStorage.removeItem('token');
  window.location.href = 'index.html';
});
