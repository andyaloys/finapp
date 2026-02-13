# Build and Push Docker Images to Docker Hub

$BACKEND_IMAGE = "andyaloys/finapp-backend"
$FRONTEND_IMAGE = "andyaloys/finapp-frontend"
$VERSION = "latest"

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "FINAPP - BUILD & PUSH TO DOCKER HUB" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# Step 1: Build Backend
Write-Host ">>> [1/4] Building Backend Image..." -ForegroundColor Yellow
docker build -t ${BACKEND_IMAGE}:${VERSION} ./backend
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ Backend build failed" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Backend image built successfully" -ForegroundColor Green
Write-Host ""

# Step 2: Build Frontend
Write-Host ">>> [2/4] Building Frontend Image..." -ForegroundColor Yellow
docker build -t ${FRONTEND_IMAGE}:${VERSION} ./frontend
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ Frontend build failed" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Frontend image built successfully" -ForegroundColor Green
Write-Host ""

# Step 3: Push Backend
Write-Host ">>> [3/4] Pushing Backend Image..." -ForegroundColor Yellow
docker push ${BACKEND_IMAGE}:${VERSION}
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ Backend push failed. Please run 'docker login' first" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Backend image pushed successfully" -ForegroundColor Green
Write-Host ""

# Step 4: Push Frontend
Write-Host ">>> [4/4] Pushing Frontend Image..." -ForegroundColor Yellow
docker push ${FRONTEND_IMAGE}:${VERSION}
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ Frontend push failed" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Frontend image pushed successfully" -ForegroundColor Green
Write-Host ""

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "✓ ALL DONE! Images pushed to Docker Hub" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Backend:  $BACKEND_IMAGE:$VERSION" -ForegroundColor White
Write-Host "Frontend: $FRONTEND_IMAGE:$VERSION" -ForegroundColor White
