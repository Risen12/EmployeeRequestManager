const API_BASE = '';

document.addEventListener('DOMContentLoaded', () => {
    setupFilterCheckboxes();
    setupCreateForm();
    setupUpdateForm();
    loadRequests();
});

function setupFilterCheckboxes() {
    const filters = [
        { checkbox: 'enableStatusFilter', input: 'statusFilter' },
        { checkbox: 'enableExecutorFilter', input: 'executorFilter' }, // Теперь это ID
        { checkbox: 'enableDepartmentFilter', input: 'departmentFilter' }
    ];

    filters.forEach(({ checkbox, input }) => {
        const checkboxEl = document.getElementById(checkbox);
        const inputEl = document.getElementById(input);

        checkboxEl.addEventListener('change', () => {
            inputEl.disabled = !checkboxEl.checked;
            if (!checkboxEl.checked) {
                inputEl.value = '';
            }
        });
    });
}

async function loadRequests() {
    const params = new URLSearchParams();
    
    if (document.getElementById('enableStatusFilter').checked) {
        const status = mapStatusForRequest(document.getElementById('statusFilter').value);
        if (status) params.append('Status', status);
    }
    
    if (document.getElementById('enableExecutorFilter').checked) {
        const executorId = document.getElementById('executorFilter').value;
        if (executorId) params.append('ExecutorId', executorId);
    }
    
    if (document.getElementById('enableDepartmentFilter').checked) {
        const department = document.getElementById('departmentFilter').value;
        if (department) params.append('Department', department);
    }
    
    if (document.getElementById('enableOverdueFilter').checked) {
        params.append('IsOverdue', 'true');
    }

    try {
        showLoading(true);
        const response = await fetch(`${API_BASE}/Requests?${params.toString()}`);

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        renderTable(data);
    } catch (error) {
        console.error('Ошибка загрузки заявок:', error);
        showError('Не удалось загрузить заявки');
    } finally {
        showLoading(false);
    }
}

function renderTable(requests) {
    const tbody = document.querySelector('#requestsTable tbody');
    tbody.innerHTML = '';

    if (!requests || requests.length === 0) {
        tbody.innerHTML = '<tr><td colspan="7" class="empty-message">Заявки не найдены</td></tr>';
        return;
    }

    requests.forEach(req => {
        const tr = document.createElement('tr');
        const statusClass = getStatusClass(req.status);
        const statusText = getStatusText(req.status);
        
        tr.innerHTML = `
            <td>${req.id}</td>
            <td>${formatDate(req.requestCreationDate)}</td>
            <td>${req.authorName || `ID: ${req.authorId}`}</td>
            <td>${req.executorName || `ID: ${req.executorId}`}</td>
            <td>${req.description}</td>
            <td><span class="status-badge ${statusClass}">${statusText}</span></td>
            <td>${formatDate(req.requestExpirationDate)}</td>
        `;
        tbody.appendChild(tr);
    });
}

function setupCreateForm() {
    document.getElementById('createForm').addEventListener('submit', async (e) => {
        e.preventDefault();
        
        const requestData = {
            description: document.getElementById('createDescription').value,
            requestExpirationDate: document.getElementById('createDeadline').value,
            authorId: parseInt(document.getElementById('createAuthorId').value),
            executorId: parseInt(document.getElementById('createExecutorId').value),
            status: "Новая"
        };

        try {
            const response = await fetch(`${API_BASE}/Requests`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(requestData)
            });

            if (response.ok) {
                alert('✅ Заявка успешно создана!');
                document.getElementById('createForm').reset();
                loadRequests();
            } else {
                const error = await response.json();
                alert(`❌ Ошибка: ${error.title || error.message || 'Не удалось создать заявку'}`);
            }
        } catch (error) {
            console.error('Ошибка создания заявки:', error);
            alert('❌ Ошибка соединения с сервером');
        }
    });
}

function setupUpdateForm() {
    document.getElementById('updateForm').addEventListener('submit', async (e) => {
        e.preventDefault();

        const requestId = document.getElementById('updateRequestId').value;
        const statusValue = document.getElementById('updateStatus').value;
        const executorIdValue = document.getElementById('updateExecutorId').value;

        const updateData = {
            newStatus: statusValue === "" ? null : statusValue,
            
            newExecutorId: executorIdValue ? parseInt(executorIdValue) : null
        };

        try {
            const response = await fetch(`${API_BASE}/Requests/${requestId}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(updateData)
            });

            if (response.ok) {
                alert('✅ Заявка успешно обновлена!');
                document.getElementById('updateForm').reset();
                loadRequests(); // Обновляем таблицу
            } else {
                const error = await response.json();
                alert(`❌ Ошибка: ${error.title || error.message || 'Не удалось обновить заявку'}`);
            }
        } catch (error) {
            console.error('Ошибка обновления заявки:', error);
            alert(`❌ Ошибка: ${error.message}`);
        }
    });
}

async function getReport(reportType) {
    const resultDiv = document.getElementById('reportResult');
    resultDiv.classList.add('show');
    resultDiv.innerHTML = '<p class="loading">Загрузка отчёта...</p>';

    const reportRoutes = {
        'by-status': '/Reports/RequestByStatus',
        'overdue-count': '/Reports/OverdueRequests',
        'by-executor': '/Reports/CompletedRequestsByExecutor'
    };

    const route = reportRoutes[reportType];

    try {
        const response = await fetch(`${API_BASE}${route}`);

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();

        resultDiv.innerHTML = `
            <h3>Результат отчёта:</h3>
            <pre>${JSON.stringify(data, null, 2)}</pre>
        `;
    } catch (error) {
        console.error('Ошибка получения отчёта:', error);
        resultDiv.innerHTML = `<p style="color: red;">❌ Ошибка: ${error.message}</p>`;
    }
}

function getStatusClass(status) {
    const statusMap = {
        'New': 'status-new',
        'InProgress': 'status-inprogress',
        'Done': 'status-done',
    };
    return statusMap[status] || 'status-new';
}

function mapStatusForRequest(status)
{
    const statusMap = {
        0 : 'Новая',
        1 : 'В работе',
        2 : 'Завершена'
    }
    
    return statusMap[status];
}

function getStatusText(status) {
    const statusMap = {
        'New': 'Новая',
        'InProgress': 'В работе',
        'Completed': 'Выполнена',
    };
    return statusMap[status] || status;
}

function formatDate(dateString) {
    if (!dateString) return 'N/A';

    if (!dateString.endsWith('Z') && !dateString.match(/[+-]\d{2}:\d{2}$/)) {
        dateString += 'Z';
    }

    const date = new Date(dateString);

    return date.toLocaleString('ru-RU', {
        timeZone: 'Europe/Moscow',
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
}

function showLoading(isLoading) {
    const tbody = document.querySelector('#requestsTable tbody');
    if (isLoading) {
        tbody.innerHTML = '<tr><td colspan="7" class="empty-message loading">Загрузка...</td></tr>';
    }
}

function showError(message) {
    const tbody = document.querySelector('#requestsTable tbody');
    tbody.innerHTML = `<tr><td colspan="7" class="empty-message" style="color: red;">${message}</td></tr>`;
}