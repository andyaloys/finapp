# ================================================
# SCRIPT DEPLOY FINAPP KE DOCKER HUB
# Docker Hub: andyaloys
# Server: 10.100.83.166
# ================================================

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "FINAPP - BUILD & PUSH TO DOCKER HUB" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# Konfigurasi
$DOCKER_USERNAME = "andyaloys"
$BACKEND_IMAGE = "$DOCKER_USERNAME/finapp-backend"
$FRONTEND_IMAGE = "$DOCKER_USERNAME/finapp-frontend"
$VERSION = "latest"

# ================================================
# 1. BUILD BACKEND IMAGE
# ================================================
Write-Host ">>> [1/4] Building Backend Image..." -ForegroundColor Yellow
docker build -t ${BACKEND_IMAGE}:${VERSION} ./backend
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Backend build failed!" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Backend image built successfully" -ForegroundColor Green
Write-Host ""

# ================================================
# 2. BUILD FRONTEND IMAGE
# ================================================
Write-Host ">>> [2/4] Building Frontend Image..." -ForegroundColor Yellow
docker build -t ${FRONTEND_IMAGE}:${VERSION} ./frontend
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Frontend build failed!" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Frontend image built successfully" -ForegroundColor Green
Write-Host ""

# ================================================
# 3. LOGIN TO DOCKER HUB
# ================================================
Write-Host ">>> [3/4] Logging in to Docker Hub..." -ForegroundColor Yellow
Write-Host "Username: $DOCKER_USERNAME" -ForegroundColor Cyan
docker login -u $DOCKER_USERNAME
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Docker Hub login failed!" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Logged in successfully" -ForegroundColor Green
Write-Host ""

# ================================================
# 4. PUSH IMAGES TO DOCKER HUB
# ================================================
Write-Host ">>> [4/4] Pushing Images to Docker Hub..." -ForegroundColor Yellow

Write-Host "  - Pushing backend image..." -ForegroundColor Cyan
docker push ${BACKEND_IMAGE}:${VERSION}
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Backend push failed!" -ForegroundColor Red
    exit 1
}

Write-Host "  - Pushing frontend image..." -ForegroundColor Cyan
docker push ${FRONTEND_IMAGE}:${VERSION}
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Frontend push failed!" -ForegroundColor Red
    exit 1
}

Write-Host "✓ All images pushed successfully" -ForegroundColor Green
Write-Host ""

# ================================================
# SUMMARY
# ================================================
Write-Host "=====================================" -ForegroundColor Green
Write-Host "✓ DEPLOYMENT IMAGES READY!" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
Write-Host ""
Write-Host "Images pushed to Docker Hub:" -ForegroundColor Cyan
Write-Host "  - ${BACKEND_IMAGE}:${VERSION}" -ForegroundColor White
Write-Host "  - ${FRONTEND_IMAGE}:${VERSION}" -ForegroundColor White
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "1. Transfer 'docker-compose.production.yml' to server" -ForegroundColor White
Write-Host "2. On server, run: docker-compose -f docker-compose.production.yml pull" -ForegroundColor White
Write-Host "3. On server, run: docker-compose -f docker-compose.production.yml up -d" -ForegroundColor White
Write-Host "4. Access:" -ForegroundColor White
Write-Host "   - Frontend: http://10.100.83.166:8008" -ForegroundColor Cyan
Write-Host "   - Backend: http://10.100.83.166:8081/api" -ForegroundColor Cyan
Write-Host ""
