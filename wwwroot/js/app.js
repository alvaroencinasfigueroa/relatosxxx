// ========== i18n ==========
const translations = {
    en: {
        age_title: 'Adults Only',
        age_text: 'This site contains explicit content for adults only. You must be 18 years or older to enter.',
        age_yes: "I'm 18+ — Enter",
        age_no: "I'm under 18 — Leave",
        logo_sub: '🔥 Forbidden Stories 🔥',
        btn_login: 'Login',
        btn_vip: 'VIP',
        hero_title: '🔥 SENSUAL STORIES 🔥',
        hero_sub: 'Where fantasy becomes reality... no limits',
        admin_title: 'God Mode',
        admin_new: 'New Story',
        section_categories: 'Categories',
        cat_all: 'All',
        cat_college: 'College',
        cat_lesbian: 'Lesbian',
        cat_booty: 'Booty',
        cat_busty: 'Busty',
        cat_darkfantasy: 'Dark Fantasy',
        cat_trans: 'Trans',
        cat_bizarre: 'Bizarre',
        filter_all: 'All',
        filter_free: 'Free',
        filter_exclusive: 'Exclusive',
        login_title: 'Sign In',
        login_sub: 'Your personal account',
        register_title: 'Sign Up',
        register_sub: 'Join the dark side',
        field_password: 'Password',
        field_name: 'Name',
        field_title: 'Title',
        field_image: 'Image URL',
        field_category: 'Category',
        field_content: 'Content',
        field_exclusive: 'Exclusive Content (VIP)',
        form_new: 'New Story',
        form_sub: 'Write your forbidden story',
        btn_edit: 'Edit',
        btn_delete: 'Delete',
        btn_save: 'Save',
        vip_title: 'VIP Access',
        vip_sub: 'Unlock ALL content',
        vip_note: 'One-time payment · Lifetime access',
        vip_benefit1: 'ALL stories, no limits',
        vip_benefit2: 'New stories every week',
        vip_benefit3: 'No monthly subscriptions',
        vip_benefit4: '100% secure crypto payment',
        btn_pay_ton: 'Pay 25 TON with Tonkeeper',
        payment_instructions: 'Payment Instructions',
        ton_step1: '1. Open Tonkeeper and send exactly 25 TON to this address:',
        ton_step2: 'IMPORTANT: Include this exact MEMO:',
        btn_verify_ton: 'I sent it, verify now',
        or_usdt: '— Or pay with USDT —',
        btn_pay_usdt: 'Pay 25 USDT (TRC20 / Bybit)',
        usdt_instructions: 'USDT Payment Instructions',
        usdt_step1: '1. Send exactly 25 USDT via TRC20 (Tron) network to this address:',
        usdt_step2: '2. After sending, paste the transaction Hash (TxID) here:',
        btn_verify_usdt: 'Verify payment',
        secure_payment: 'Secure payment · TON & USDT',
        welcome: 'Welcome! 🔥',
        session_closed: 'Session closed',
        vip_welcome: 'Welcome to VIP! 🔥',
        payment_confirmed: 'Payment confirmed!',
        login_required: 'You must log in to read exclusive stories',
        go_vip: 'Get VIP to unlock this story! 🔥',
        login_to_buy: 'You must log in to purchase Premium.',
        loading_address: 'Loading...',
        copied: 'Address copied!',
        error_address: 'Error getting address',
        error_generating: 'Error generating payment',
        verify_blockchain: 'Verifying on blockchain...',
        ton_not_found: 'Payment not found yet. If you just sent it, wait a minute.',
        error_connection: 'Connection error.',
        story_updated: 'Story updated 🔥',
        story_created: 'Story created 🔥',
        story_deleted: 'Story deleted',
        connecting: 'Connecting...',
        connecting_text: 'Establishing server connection',
        no_stories: 'No stories here',
        no_stories_text: 'Explore other categories or come back later',
        locked_preview: 'This content is exclusive for VIP subscribers...',
        vip_tag: 'VIP',
    },
    es: {
        age_title: 'Solo Adultos',
        age_text: 'Este sitio contiene contenido explícito solo para adultos. Debes tener 18 años o más para entrar.',
        age_yes: 'Tengo 18+ — Entrar',
        age_no: 'Tengo menos de 18 — Salir',
        logo_sub: '🔥 Historias Prohibidas 🔥',
        btn_login: 'Entrar',
        btn_vip: 'VIP',
        hero_title: '🔥 RELATOS SENSUALES 🔥',
        hero_sub: 'Donde la fantasía se vuelve realidad... sin límites',
        admin_title: 'Modo Dios',
        admin_new: 'Nuevo Relato',
        section_categories: 'Categorías',
        cat_all: 'Todas',
        cat_college: 'Universitarios',
        cat_lesbian: 'Lesbianas',
        cat_booty: 'Nalgonas',
        cat_busty: 'Tetonas',
        cat_darkfantasy: 'Fantasía Oscura',
        cat_trans: 'Transexuales',
        cat_bizarre: 'Bizarro',
        filter_all: 'Todos',
        filter_free: 'Gratuitos',
        filter_exclusive: 'Exclusivos',
        login_title: 'Acceder',
        login_sub: 'Tu cuenta personal',
        register_title: 'Registrarse',
        register_sub: 'Únete al lado oscuro',
        field_password: 'Contraseña',
        field_name: 'Nombre',
        field_title: 'Título',
        field_image: 'URL de Imagen',
        field_category: 'Categoría',
        field_content: 'Contenido',
        field_exclusive: 'Contenido Exclusivo (VIP)',
        form_new: 'Nuevo Relato',
        form_sub: 'Escribe tu historia prohibida',
        btn_edit: 'Editar',
        btn_delete: 'Eliminar',
        btn_save: 'Guardar',
        vip_title: 'Acceso VIP',
        vip_sub: 'Desbloquea TODO el contenido',
        vip_note: 'Pago único · Acceso de por vida',
        vip_benefit1: 'TODOS los relatos sin límites',
        vip_benefit2: 'Historias nuevas cada semana',
        vip_benefit3: 'Sin suscripciones mensuales',
        vip_benefit4: 'Pago 100% seguro con cripto',
        btn_pay_ton: 'Pagar 25 TON con Tonkeeper',
        payment_instructions: 'Instrucciones de Pago',
        ton_step1: '1. Abre Tonkeeper y envía exactamente 25 TON a esta dirección:',
        ton_step2: 'IMPORTANTE: Incluye este MEMO exacto:',
        btn_verify_ton: 'Ya hice el envío, verificar',
        or_usdt: '— O paga con USDT —',
        btn_pay_usdt: 'Pagar 25 USDT (TRC20 / Bybit)',
        usdt_instructions: 'Instrucciones de Pago USDT',
        usdt_step1: '1. Envía exactamente 25 USDT por la red TRC20 (Tron) a esta dirección:',
        usdt_step2: '2. Después de enviar, pega el Hash de la transacción (TxID) aquí:',
        btn_verify_usdt: 'Verificar pago',
        secure_payment: 'Pago seguro · TON & USDT',
        welcome: '¡Bienvenido/a! 🔥',
        session_closed: 'Sesión cerrada',
        vip_welcome: '¡Bienvenido al club VIP! 🔥',
        payment_confirmed: '¡Pago confirmado!',
        login_required: 'Debes iniciar sesión para leer relatos exclusivos',
        go_vip: '¡Hazte VIP para desbloquear este relato! 🔥',
        login_to_buy: 'Debes iniciar sesión para comprar Premium.',
        loading_address: 'Cargando...',
        copied: '¡Dirección copiada!',
        error_address: 'Error al obtener la dirección',
        error_generating: 'Error al generar los datos de pago',
        verify_blockchain: 'Verificando en la blockchain...',
        ton_not_found: 'Aún no vemos el pago. Si acabas de pagar, espera un minuto.',
        error_connection: 'Error de conexión.',
        story_updated: 'Relato actualizado 🔥',
        story_created: 'Relato creado 🔥',
        story_deleted: 'Relato eliminado',
        connecting: 'Conectando...',
        connecting_text: 'Estableciendo conexión con el servidor',
        no_stories: 'No hay historias aquí',
        no_stories_text: 'Explora otras categorías o vuelve más tarde',
        locked_preview: 'Este contenido es exclusivo para suscriptores VIP...',
        vip_tag: 'VIP',
    }
};

let currentLang = localStorage.getItem('lang') || 'en';

function i18n(key) {
    return translations[currentLang][key] || translations['en'][key] || key;
}

function setLang(lang) {
    currentLang = lang;
    localStorage.setItem('lang', lang);
    document.documentElement.lang = lang;
    applyTranslations();
    document.getElementById('btn-lang-en').classList.toggle('active', lang === 'en');
    document.getElementById('btn-lang-es').classList.toggle('active', lang === 'es');
    displayRelatos();
}

function applyTranslations() {
    document.querySelectorAll('[data-i18n]').forEach(el => {
        const key = el.getAttribute('data-i18n');
        const val = i18n(key);
        if (el.tagName === 'INPUT' || el.tagName === 'TEXTAREA') {
            el.placeholder = val;
        } else {
            el.textContent = val;
        }
    });
    document.title = currentLang === 'es' ? '🔥 RELATOS SENSUALES 🔥' : '🔥 SENSUAL STORIES 🔥';
}

// ========== CONFIG ==========
const API_URL = window.location.origin + '/api';

// ========== FETCH AUTENTICADO ==========
async function fetchAuth(url, options = {}) {
    const token = localStorage.getItem('token');
    const response = await fetch(url, {
        ...options,
        headers: {
            ...options.headers,
            'Authorization': `Bearer ${token}`
        }
    });
    if (response.status === 401) {
        logout();
        toast(currentLang === 'es' ? 'Sesión expirada, inicia sesión de nuevo' : 'Session expired, please log in again', 'error');
        showModal('loginModal');
        throw new Error('Unauthorized');
    }
    return response;
}

// ========== STATE ==========
let currentUser = null;
let currentRelato = null;
let allRelatos = [];
let currentFilter = 'todos';
let currentCategory = 'todas';
let currentPage = 1;
const PAGE_SIZE = 12;
let totalPages = 1;

const categoryKeys = {
    'universitarios': 'cat_college',
    'lesbianas': 'cat_lesbian',
    'nalgonas': 'cat_booty',
    'tetonas': 'cat_busty',
    'noconsentido': 'cat_darkfantasy',
    'transexuales': 'cat_trans',
    'bizarro': 'cat_bizarre'
};

// ========== AMBIENT IMAGES ==========
let ambientInterval;

async function loadAmbientImages() {
    try {
        const fetchImg = () => fetch(`${API_URL}/Imagenes/random`).then(r => r.json()).catch(() => null);
        const results = await Promise.all([fetchImg(), fetchImg(), fetchImg(), fetchImg()]);
        const leftUrls = [results[0]?.url, results[1]?.url].filter(Boolean);
        const rightUrls = [results[2]?.url, results[3]?.url].filter(Boolean);
        if (leftUrls.length > 0) setAmbientImages('left', leftUrls);
        if (rightUrls.length > 0) setAmbientImages('right', rightUrls);
    } catch (err) {
        console.error('Error loading ambient images', err);
    }
}

function setAmbientImages(side, urls) {
    const container = document.getElementById(side === 'left' ? 'ambientLeft' : 'ambientRight');
    if (!container) return;
    const existing = container.querySelectorAll('img');
    if (existing.length === urls.length) {
        existing.forEach((img, i) => {
            img.classList.add('fade-out');
            setTimeout(() => { img.src = urls[i]; img.classList.remove('fade-out'); }, 800);
        });
    } else {
        container.innerHTML = '';
        urls.forEach(url => {
            if (!url) return;
            const img = document.createElement('img');
            img.src = url;
            img.alt = '';
            container.appendChild(img);
        });
    }
}

function startAmbientTimer() {
    if (ambientInterval) clearInterval(ambientInterval);
    loadAmbientImages();
    ambientInterval = setInterval(loadAmbientImages, 30000);
}

function stopAmbientTimer() {
    if (ambientInterval) {
        clearInterval(ambientInterval);
        ambientInterval = null;
    }
    document.getElementById('ambientLeft').innerHTML = '';
    document.getElementById('ambientRight').innerHTML = '';
}

// ========== SVG POR CATEGORÍA ==========
const categoryHearts = {
    'universitarios': { bg: 'linear-gradient(135deg,#1a1a2e,#16213e)', heart: ['#ff69b4', '#ff1493'], glow: 'rgba(255,20,147,0.4)' },
    'lesbianas': { bg: 'linear-gradient(135deg,#3d0030,#7b0050)', heart: ['#ff69b4', '#ff00aa'], glow: 'rgba(255,0,170,0.5)' },
    'nalgonas': { bg: 'linear-gradient(135deg,#1a0005,#3d0010)', heart: ['#ff006e', '#ff1493'], glow: 'rgba(255,0,110,0.5)' },
    'tetonas': { bg: 'linear-gradient(135deg,#200010,#500030)', heart: ['#ff4d6d', '#c9184a'], glow: 'rgba(201,24,74,0.5)' },
    'noconsentido': { bg: 'linear-gradient(135deg,#0d0d0d,#1a0000)', heart: ['#8b0000', '#3d0000'], glow: 'rgba(139,0,0,0.5)' },
    'transexuales': { bg: 'linear-gradient(135deg,#0a0020,#1a0040)', heart: ['#9b59b6', '#6c3483'], glow: 'rgba(155,89,182,0.5)' },
    'bizarro': { bg: 'linear-gradient(135deg,#0a0a0a,#1a1a00)', heart: ['#39ff14', '#ff006e'], glow: 'rgba(57,255,20,0.3)' }
};

function getHeartSvg(categoria, isLocked) {
    const theme = categoryHearts[categoria] || categoryHearts['nalgonas'];
    const [c1, c2] = theme.heart;
    const lockOverlay = isLocked ? `
        <rect width="200" height="200" fill="rgba(0,0,0,0.55)"/>
        <text x="100" y="95" text-anchor="middle" font-size="32" fill="#fff">🔒</text>
        <text x="100" y="125" text-anchor="middle" font-family="sans-serif" font-size="13" fill="rgba(255,179,198,0.9)">VIP Only</text>
    ` : '';
    return `
        <div style="position:relative;height:200px;overflow:hidden;background:${theme.bg};display:flex;align-items:center;justify-content:center;">
            <svg width="200" height="200" viewBox="0 0 200 200" xmlns="http://www.w3.org/2000/svg" style="filter:drop-shadow(0 0 18px ${theme.glow})">
                <defs>
                    <radialGradient id="hg_${categoria}" cx="50%" cy="40%" r="60%">
                        <stop offset="0%" stop-color="${c1}"/>
                        <stop offset="100%" stop-color="${c2}"/>
                    </radialGradient>
                </defs>
                <path d="M100 160 C60 130 20 105 20 72 C20 45 40 28 62 28 C78 28 92 38 100 50 C108 38 122 28 138 28 C160 28 180 45 180 72 C180 105 140 130 100 160Z"
                      fill="url(#hg_${categoria})" opacity="0.95"/>
                <path d="M100 145 C68 120 35 100 35 72 C35 52 50 40 65 40 C78 40 90 48 100 60"
                      fill="none" stroke="rgba(255,255,255,0.15)" stroke-width="2"/>
                ${lockOverlay}
            </svg>
            <div style="position:absolute;inset:0;background:linear-gradient(180deg,transparent 40%,rgba(0,0,0,0.85) 100%);"></div>
        </div>
    `;
}

// ========== HELPERS ==========
function escapeHtml(text) {
    if (!text) return '';
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

// ========== INIT ==========
document.addEventListener('DOMContentLoaded', function () {
    applyTranslations();
    checkAgeGate();
    checkUserSession();
    loadRelatos();
    createParticles();
    initPayments();
});

// ========== AGE GATE ==========
function checkAgeGate() {
    const confirmed = localStorage.getItem('ageConfirmed');
    if (!confirmed) {
        document.getElementById('ageGate').style.display = 'flex';
        document.body.style.overflow = 'hidden';
    }
}

function confirmAge(isAdult) {
    if (isAdult) {
        localStorage.setItem('ageConfirmed', 'true');
        document.getElementById('ageGate').style.display = 'none';
        document.body.style.overflow = '';
    } else {
        window.location.href = 'https://www.google.com';
    }
}

// ========== PARTICLES ==========
function createParticles() {
    const container = document.getElementById('particles');
    for (let i = 0; i < 20; i++) {
        const p = document.createElement('div');
        p.className = 'particle';
        p.style.left = Math.random() * 100 + '%';
        p.style.animationDelay = Math.random() * 15 + 's';
        p.style.animationDuration = (10 + Math.random() * 10) + 's';
        container.appendChild(p);
    }
}

// ========== SESSION ==========
function checkUserSession() {
    const token = localStorage.getItem('token');
    const userData = localStorage.getItem('userData');
    if (token && userData) {
        currentUser = JSON.parse(userData);
        updateUserInterface();
    }
}

function updateUserInterface() {
    const userSection = document.getElementById('userSection');
    const adminPanel = document.getElementById('adminPanel');

    if (currentUser) {
        const initial = currentUser.nombre ? currentUser.nombre.charAt(0).toUpperCase() : 'U';
        userSection.innerHTML = `
            <div class="user-bar">
                <div class="user-card">
                    <div class="user-avatar">${initial}</div>
                    <span>${escapeHtml(currentUser.nombre)}</span>
                    ${currentUser.isAdmin ? `<span class="tag tag-gold">ADMIN</span>` : ''}
                    ${currentUser.isPremium ? `<span class="tag tag-pink">${i18n('vip_tag')}</span>` : ''}
                </div>
                <button class="btn btn-dark btn-sm" onclick="logout()">
                    <i class="fas fa-sign-out-alt"></i>
                </button>
            </div>
        `;
        adminPanel.classList.toggle('active', currentUser.isAdmin);
    } else {
        userSection.innerHTML = `
            <div style="display:flex; gap:12px;">
                <button class="btn btn-outline-pink" onclick="showModal('loginModal')">
                    <i class="fas fa-key"></i> ${i18n('btn_login')}
                </button>
                <button class="btn btn-fire" onclick="showModal('registerModal')">
                    <i class="fas fa-crown"></i> ${i18n('btn_vip')}
                </button>
            </div>
        `;
        adminPanel.classList.remove('active');
    }
}

// ========== AUTH ==========
async function login(event) {
    event.preventDefault();
    const email = document.getElementById('loginEmail').value;
    const password = document.getElementById('loginPassword').value;
    try {
        const response = await fetch(`${API_URL}/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ Email: email, Password: password })
        });
        const data = await response.json();
        if (!response.ok) throw new Error(data.message || 'Error');
        localStorage.setItem('token', data.token);
        localStorage.setItem('userData', JSON.stringify(data.user));
        currentUser = data.user;
        closeModal('loginModal');
        updateUserInterface();
        loadRelatos();
        toast(i18n('welcome'), 'success');
    } catch (error) {
        toast(error.message, 'error');
    }
}

async function register(event) {
    event.preventDefault();
    const nombre = document.getElementById('registerName').value;
    const email = document.getElementById('registerEmail').value;
    const password = document.getElementById('registerPassword').value;
    try {
        const response = await fetch(`${API_URL}/auth/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ Nombre: nombre, Email: email, Password: password })
        });
        const data = await response.json();
        if (!response.ok) throw new Error(data.message || 'Error');
        const loginResponse = await fetch(`${API_URL}/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ Email: email, Password: password })
        });
        const loginData = await loginResponse.json();
        if (loginResponse.ok) {
            localStorage.setItem('token', loginData.token);
            localStorage.setItem('userData', JSON.stringify(loginData.user));
            currentUser = loginData.user;
        }
        closeModal('registerModal');
        updateUserInterface();
        showPaypalModal();
    } catch (error) {
        toast(error.message, 'error');
    }
}

function logout() {
    localStorage.clear();
    currentUser = null;
    updateUserInterface();
    loadRelatos();
    toast(i18n('session_closed'), 'info');
}

// ========== RELATOS ==========
async function loadRelatos(page = 1) {
    currentPage = page;
    try {
        const response = await fetch(`${API_URL}/Relatos?page=${page}&pageSize=${PAGE_SIZE}`);
        if (!response.ok) throw new Error('Error');
        const result = await response.json();
        allRelatos = result.data;
        totalPages = result.totalPages;
        displayRelatos();
        renderPaginacion();
    } catch (error) {
        document.getElementById('relatosGrid').innerHTML = `
            <div class="empty-box" style="grid-column:1/-1;">
                <div class="empty-icon"><i class="fas fa-plug"></i></div>
                <h3 class="empty-title">${i18n('connecting')}</h3>
                <p class="empty-text">${i18n('connecting_text')}</p>
            </div>
        `;
    }
}

function displayRelatos() {
    const grid = document.getElementById('relatosGrid');
    let filtered = allRelatos;

    if (currentCategory !== 'todas') {
        filtered = filtered.filter(r => r.categoria === currentCategory);
    }
    if (currentFilter === 'gratis') {
        filtered = filtered.filter(r => !r.esPremium);
    } else if (currentFilter === 'premium') {
        filtered = filtered.filter(r => r.esPremium);
    }

    if (!filtered || filtered.length === 0) {
        grid.innerHTML = `
            <div class="empty-box" style="grid-column:1/-1;">
                <div class="empty-icon"><i class="fas fa-book-dead"></i></div>
                <h3 class="empty-title">${i18n('no_stories')}</h3>
                <p class="empty-text">${i18n('no_stories_text')}</p>
            </div>
        `;
        return;
    }

    grid.innerHTML = filtered.map(relato => {
        const isLocked = relato.esPremium && (!currentUser || (!currentUser.isPremium && !currentUser.isAdmin));
        const preview = isLocked
            ? i18n('locked_preview')
            : (relato.contenido ? relato.contenido.substring(0, 100) + '...' : '...');
        const catLabel = categoryKeys[relato.categoria] ? i18n(categoryKeys[relato.categoria]) : relato.categoria;
        const safeTitle = escapeHtml(relato.titulo);
        const safePreview = escapeHtml(preview);
        const safeCatLabel = escapeHtml(catLabel);

        return `
            <div class="story-card ${relato.esPremium ? 'story-vip' : ''} ${isLocked ? 'locked-card' : ''}" onclick="openRelato(${relato.id})">
                ${getHeartSvg(relato.categoria, isLocked)}
                ${relato.esPremium ? `<div class="vip-crown" style="position:absolute;top:12px;right:12px;"><i class="fas fa-crown"></i> ${i18n('vip_tag')}</div>` : ''}
                <div class="card-body">
                    <span class="card-cat">${safeCatLabel}</span>
                    <h3 class="card-title">${safeTitle}</h3>
                    <p class="card-preview">${safePreview}</p>
                </div>
            </div>
        `;
    }).join('');
}

function renderPaginacion() {
    let container = document.getElementById('paginacion');
    if (!container) {
        container = document.createElement('div');
        container.id = 'paginacion';
        container.style.cssText = 'display:flex; justify-content:center; align-items:center; gap:10px; margin:20px 0 60px;';
        document.getElementById('relatosGrid').insertAdjacentElement('afterend', container);
    }

    if (totalPages <= 1) { container.innerHTML = ''; return; }

    let html = `
        <button onclick="loadRelatos(${currentPage - 1})"
            style="padding:10px 20px; background:${currentPage === 1 ? 'rgba(255,255,255,0.05)' : 'rgba(255,0,110,0.2)'}; 
            border:1px solid rgba(255,0,110,0.3); color:var(--pink-soft); cursor:${currentPage === 1 ? 'not-allowed' : 'pointer'}; 
            font-family:'Righteous',sans-serif; font-size:12px; letter-spacing:1px;"
            ${currentPage === 1 ? 'disabled' : ''}>← Anterior</button>
    `;

    for (let i = 1; i <= totalPages; i++) {
        if (i === 1 || i === totalPages || (i >= currentPage - 1 && i <= currentPage + 1)) {
            html += `
                <button onclick="loadRelatos(${i})"
                    style="padding:10px 16px; 
                    background:${i === currentPage ? 'var(--pink-vivid)' : 'rgba(255,255,255,0.05)'}; 
                    border:1px solid ${i === currentPage ? 'var(--pink-vivid)' : 'rgba(255,0,110,0.2)'}; 
                    color:${i === currentPage ? 'white' : 'var(--pink-soft)'}; 
                    cursor:pointer; font-family:'Righteous',sans-serif; font-size:12px;">${i}</button>
            `;
        } else if (i === currentPage - 2 || i === currentPage + 2) {
            html += `<span style="color:var(--pink-soft); padding:0 4px;">...</span>`;
        }
    }

    html += `
        <button onclick="loadRelatos(${currentPage + 1})"
            style="padding:10px 20px; background:${currentPage === totalPages ? 'rgba(255,255,255,0.05)' : 'rgba(255,0,110,0.2)'}; 
            border:1px solid rgba(255,0,110,0.3); color:var(--pink-soft); cursor:${currentPage === totalPages ? 'not-allowed' : 'pointer'}; 
            font-family:'Righteous',sans-serif; font-size:12px; letter-spacing:1px;"
            ${currentPage === totalPages ? 'disabled' : ''}>Siguiente →</button>
    `;

    container.innerHTML = html;
}

function openRelato(id) {
    const relato = allRelatos.find(r => r.id === id);
    if (!relato) return;

    const isLocked = relato.esPremium && (!currentUser || (!currentUser.isPremium && !currentUser.isAdmin));
    if (isLocked) {
        if (!currentUser) {
            toast(i18n('login_required'), 'info');
            showModal('loginModal');
        } else {
            toast(i18n('go_vip'), 'info');
            showPaypalModal();
        }
        return;
    }

    currentRelato = relato;
    document.getElementById('relatoTitle').textContent = relato.titulo;
    const catLabel = categoryKeys[relato.categoria] ? i18n(categoryKeys[relato.categoria]) : relato.categoria;
    document.getElementById('relatoCategory').textContent = catLabel;
    document.getElementById('relatoContent').textContent = relato.contenido;

    const imgEl = document.getElementById('relatoImage');
    if (relato.imagenUrl) { imgEl.src = relato.imagenUrl; imgEl.style.display = 'block'; }
    else { imgEl.style.display = 'none'; }

    document.getElementById('relatoActions').style.display = (currentUser && currentUser.isAdmin) ? 'flex' : 'none';

    showModal('relatoModal');
    startAmbientTimer();
}

// ========== CRUD ==========
async function saveRelato(event) {
    event.preventDefault();
    const id = document.getElementById('relatoId').value;
    const data = {
        Titulo: document.getElementById('relatoTitulo').value,
        Categoria: document.getElementById('relatoCategoria').value,
        Contenido: document.getElementById('relatoTexto').value,
        ImagenUrl: document.getElementById('relatoImagen').value,
        EsPremium: document.getElementById('relatoPremium').checked
    };
    try {
        let response;
        if (id) {
            data.Id = parseInt(id);
            response = await fetchAuth(`${API_URL}/Relatos/${id}`, { method: 'PUT', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) });
        } else {
            response = await fetchAuth(`${API_URL}/Relatos`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) });
        }
        if (!response.ok) { const err = await response.json(); throw new Error(err.message || 'Error'); }
        closeModal('createRelatoModal');
        loadRelatos();
        toast(id ? i18n('story_updated') : i18n('story_created'), 'success');
    } catch (error) {
        toast(error.message, 'error');
    }
}

function showCreateModal() {
    document.getElementById('relatoId').value = '';
    document.getElementById('relatoTitulo').value = '';
    document.getElementById('relatoCategoria').value = 'universitarios';
    document.getElementById('relatoTexto').value = '';
    document.getElementById('relatoImagen').value = '';
    document.getElementById('relatoPremium').checked = false;
    document.getElementById('formTitle').textContent = i18n('form_new');
    showModal('createRelatoModal');
}

function editCurrentRelato() {
    if (!currentRelato) return;
    closeModal('relatoModal');
    document.getElementById('relatoId').value = currentRelato.id;
    document.getElementById('relatoTitulo').value = currentRelato.titulo;
    document.getElementById('relatoCategoria').value = currentRelato.categoria;
    document.getElementById('relatoTexto').value = currentRelato.contenido;
    document.getElementById('relatoImagen').value = currentRelato.imagenUrl || '';
    document.getElementById('relatoPremium').checked = currentRelato.esPremium;
    document.getElementById('formTitle').textContent = i18n('btn_edit') + ' ' + i18n('form_new').toLowerCase();
    showModal('createRelatoModal');
}

async function deleteCurrentRelato() {
    if (!currentRelato) return;
    if (!confirm(i18n('btn_delete') + '?')) return;
    try {
        const response = await fetchAuth(`${API_URL}/Relatos/${currentRelato.id}`, { method: 'DELETE' });
        if (!response.ok) throw new Error('Error');
        closeModal('relatoModal');
        loadRelatos();
        toast(i18n('story_deleted'), 'success');
    } catch (error) {
        toast(error.message, 'error');
    }
}

// ========== FILTERS ==========
function filterByCategory(cat, event) {
    currentPage = 1;
    currentCategory = cat;
    document.querySelectorAll('.cat-btn').forEach(b => b.classList.remove('active'));
    event.target.closest('.cat-btn').classList.add('active');
    displayRelatos();
}

function filterRelatos(filter, event) {
    currentPage = 1;
    currentFilter = filter;
    document.querySelectorAll('.filter-btn').forEach(t => t.classList.remove('active'));
    event.target.closest('.filter-btn').classList.add('active');
    displayRelatos();
}

// ========== MODAL HELPERS ==========
function showModal(id) {
    document.getElementById(id).classList.add('active');
    document.body.style.overflow = 'hidden';
}

function closeModal(id) {
    if (id === 'relatoModal') stopAmbientTimer();
    document.getElementById(id).classList.remove('active');
    document.body.style.overflow = '';
}

document.addEventListener('click', function (e) {
    if (e.target.classList.contains('modal')) {
        const id = e.target.id;
        if (id === 'relatoModal') stopAmbientTimer();
        e.target.classList.remove('active');
        document.body.style.overflow = '';
    }
});

document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape') {
        document.querySelectorAll('.modal.active').forEach(m => {
            if (m.id === 'relatoModal') stopAmbientTimer();
            m.classList.remove('active');
        });
        document.body.style.overflow = '';
    }
});

// ========== TOAST ==========
function toast(message, type = 'info') {
    const existing = document.querySelector('.toast');
    if (existing) existing.remove();
    const t = document.createElement('div');
    t.className = `toast toast-${type}`;
    t.innerHTML = `<i class="fas ${type === 'success' ? 'fa-check-circle' : type === 'error' ? 'fa-exclamation-circle' : 'fa-info-circle'}"></i> ${message}`;
    document.body.appendChild(t);
    setTimeout(() => {
        t.style.animation = 'slideOutRight 0.3s ease forwards';
        setTimeout(() => t.remove(), 300);
    }, 3500);
}

// ========== VIP MODAL ==========
function showPaypalModal() {
    const btnTon = document.getElementById('btn-ton');
    const btnUsdt = document.getElementById('btn-usdt');
    const tonInfo = document.getElementById('ton-payment-info');
    const usdtInfo = document.getElementById('usdt-payment-info');
    if (btnTon) { btnTon.style.display = ''; btnTon.disabled = false; btnTon.innerHTML = `<i class="fa-solid fa-gem"></i> ${i18n('btn_pay_ton')}`; }
    if (tonInfo) tonInfo.style.display = 'none';
    if (btnUsdt) { btnUsdt.style.display = ''; btnUsdt.disabled = false; btnUsdt.innerHTML = `<i class="fa-solid fa-dollar-sign"></i> ${i18n('btn_pay_usdt')}`; }
    if (usdtInfo) usdtInfo.style.display = 'none';
    showModal('paypalModal');
}

// ========== PAYMENTS ==========
function initPayments() {
    const btnTon = document.getElementById('btn-ton');
    const btnVerificarTon = document.getElementById('btn-verificar-ton');

    if (btnTon) {
        btnTon.addEventListener('click', async () => {
            const token = localStorage.getItem('token');
            if (!token) { toast(i18n('login_to_buy'), 'error'); return; }
            btnTon.innerHTML = `<i class="fa-solid fa-spinner fa-spin"></i> ${i18n('loading_address')}`;
            btnTon.disabled = true;
            try {
                const response = await fetchAuth('/api/Payment/ton/generar-pago', { method: 'GET' });
                if (!response.ok) throw new Error(i18n('error_generating'));
                const data = await response.json();
                document.getElementById('ton-payment-info').style.display = 'block';
                btnTon.style.display = 'none';
                document.getElementById('ton-address').innerText = data.address;
                document.getElementById('ton-memo').innerText = data.memo;
                btnVerificarTon.dataset.memo = data.memo;
            } catch (error) {
                toast(error.message, 'error');
                btnTon.innerHTML = `<i class="fa-solid fa-gem"></i> ${i18n('btn_pay_ton')}`;
                btnTon.disabled = false;
            }
        });
    }

    if (btnVerificarTon) {
        btnVerificarTon.addEventListener('click', async (e) => {
            const memo = e.target.dataset.memo;
            e.target.innerHTML = `<i class="fa-solid fa-spinner fa-spin"></i> ${i18n('verify_blockchain')}`;
            e.target.disabled = true;
            try {
                const response = await fetchAuth('/api/Payment/ton/verificar-pago', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ memo: memo })
                });
                const result = await response.json();
                if (response.ok) {
                    toast(`${i18n('payment_confirmed')} ${result.message}`, 'success');
                    currentUser.isPremium = true;
                    localStorage.setItem('userData', JSON.stringify(currentUser));
                    setTimeout(() => { closeModal('paypalModal'); updateUserInterface(); loadRelatos(); }, 2000);
                } else {
                    toast(i18n('ton_not_found'), 'error');
                }
            } catch (error) {
                toast(i18n('error_connection'), 'error');
            } finally {
                e.target.innerHTML = `<i class="fa-solid fa-check-double"></i> ${i18n('btn_verify_ton')}`;
                e.target.disabled = false;
            }
        });
    }

    const btnUsdt = document.getElementById('btn-usdt');
    const btnVerificarUsdt = document.getElementById('btn-verificar-usdt');

    if (btnUsdt) {
        btnUsdt.addEventListener('click', async () => {
            const token = localStorage.getItem('token');
            if (!token) { toast(i18n('login_to_buy'), 'error'); return; }
            btnUsdt.innerHTML = `<i class="fa-solid fa-spinner fa-spin"></i> ${i18n('loading_address')}`;
            btnUsdt.disabled = true;
            try {
                const response = await fetchAuth('/api/Payment/usdt/obtener-direccion');
                if (!response.ok) throw new Error(i18n('error_address'));
                const data = await response.json();
                document.getElementById('usdt-address').innerText = data.direccion;
                document.getElementById('usdt-payment-info').style.display = 'block';
                btnUsdt.style.display = 'none';
            } catch (error) {
                toast(error.message, 'error');
                btnUsdt.innerHTML = `<i class="fa-solid fa-dollar-sign"></i> ${i18n('btn_pay_usdt')}`;
                btnUsdt.disabled = false;
            }
        });
    }

    if (btnVerificarUsdt) {
        btnVerificarUsdt.addEventListener('click', async () => {
            const txId = document.getElementById('usdt-txid-input').value.trim();
            if (!txId) { toast('TxID required', 'error'); return; }
            btnVerificarUsdt.innerHTML = `<i class="fa-solid fa-spinner fa-spin"></i> ${i18n('verify_blockchain')}`;
            btnVerificarUsdt.disabled = true;
            try {
                const response = await fetchAuth('/api/Payment/usdt/verificar-pago', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ txId: txId })
                });
                const result = await response.json();
                if (response.ok) {
                    toast(`${i18n('payment_confirmed')} ${result.message}`, 'success');
                    currentUser.isPremium = true;
                    localStorage.setItem('userData', JSON.stringify(currentUser));
                    setTimeout(() => { closeModal('paypalModal'); updateUserInterface(); loadRelatos(); }, 2000);
                } else {
                    toast(result.message, 'error');
                }
            } catch (error) {
                toast(i18n('error_connection'), 'error');
            } finally {
                btnVerificarUsdt.innerHTML = `<i class="fa-solid fa-check-double"></i> ${i18n('btn_verify_usdt')}`;
                btnVerificarUsdt.disabled = false;
            }
        });
    }
}