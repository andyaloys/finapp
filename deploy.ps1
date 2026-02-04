# Deploy FinApp to Docker Hub
# Username: andyaloys

$DOCKER_USERNAME = "andyaloys"
$BACKEND_IMAGE = "$DOCKER_USERNAME/finapp-backend"
$FRONTEND_IMAGE = "$DOCKER_USERNAME/finapp-frontend"
$VERSION = "latest"

Write-Host "Building Backend Image..." -ForegroundColor Yellow
docker build -t ${BACKEND_IMAGE}:${VERSION} ./backend
if ($LASTEXITCODE -ne 0) { exit 1 }
Write-Host "Backend built successfully" -ForegroundColor Green

Write-Host "Building Frontend Image..." -ForegroundColor Yellow
docker build -t ${FRONTEND_IMAGE}:${VERSION} ./frontend
if ($LASTEXITCODE -ne 0) { exit 1 }
Write-Host "Frontend built successfully" -ForegroundColor Green

Write-Host "Logging in to Docker Hub..." -ForegroundColor Yellow
docker login -u $DOCKER_USERNAME
if ($LASTEXITCODE -ne 0) { exit 1 }

Write-Host "Pushing Backend Image..." -ForegroundColor Yellow
docker push ${BACKEND_IMAGE}:${VERSION}
if ($LASTEXITCODE -ne 0) { exit 1 }

Write-Host "Pushing Frontend Image..." -ForegroundColor Yellow
docker push ${FRONTEND_IMAGE}:${VERSION}
if ($LASTEXITCODE -ne 0) { exit 1 }

Write-Host "SUCCESS! Images pushed to Docker Hub" -ForegroundColor Green
Write-Host "Backend: ${BACKEND_IMAGE}:${VERSION}" -ForegroundColor Cyan
Write-Host "Frontend: ${FRONTEND_IMAGE}:${VERSION}" -ForegroundColor Cyan
