# ================================================
# PANDUAN DEPLOY FINAPP KE SERVER
# Server: 10.100.83.166 (Windows)
# ================================================

## STEP 1: BUILD & PUSH IMAGE (Di Komputer Lokal)
## ================================================

1. Buka PowerShell sebagai Administrator
2. Masuk ke folder project:
   ```powershell
   cd c:\TI\NET\finapp
   ```

3. Jalankan script deploy:
   ```powershell
   .\deploy-to-dockerhub.ps1
   ```

4. Ketika diminta login Docker Hub:
   - Username: andyaloys
   - Password: [password Docker Hub Anda]

5. Tunggu hingga proses selesai (sekitar 5-10 menit)


## STEP 2: TRANSFER FILE KE SERVER
## ================================================

Transfer file ini ke server (10.100.83.166):
- docker-compose.production.yml

Cara transfer:
- Via Remote Desktop: Copy paste
- Via FTP/SFTP
- Via shared folder


## STEP 3: PULL & RUN DI SERVER
## ================================================

1. Login ke server 10.100.83.166

2. Buka PowerShell sebagai Administrator

3. Masuk ke folder tempat file docker-compose.production.yml:
   ```powershell
   cd C:\path\to\finapp
   ```

4. Pull images dari Docker Hub:
   ```powershell
   docker-compose -f docker-compose.production.yml pull
   ```

5. Run containers:
   ```powershell
   docker-compose -f docker-compose.production.yml up -d
   ```

6. Cek status containers:
   ```powershell
   docker-compose -f docker-compose.production.yml ps
   ```


## STEP 4: VERIFIKASI
## ================================================

1. Cek logs backend:
   ```powershell
   docker logs finapp-backend -f
   ```

2. Cek logs frontend:
   ```powershell
   docker logs finapp-frontend -f
   ```

3. Test akses aplikasi:
   - Frontend: http://10.100.83.166:8008
   - Backend API: http://10.100.83.166:8081/api
   - Swagger: http://10.100.83.166:8081/swagger


## STEP 5: DATABASE MIGRATION (PENTING!)
## ================================================

Jika database belum ada struktur tabelnya:

1. Jalankan migration:
   ```powershell
   docker exec -it finapp-backend dotnet ef database update
   ```

Atau koneksikan ke MySQL dan jalankan script init.sql


## TROUBLESHOOTING
## ================================================

### Container tidak bisa start:
```powershell
docker-compose -f docker-compose.production.yml logs
```

### Restart containers:
```powershell
docker-compose -f docker-compose.production.yml restart
```

### Stop containers:
```powershell
docker-compose -f docker-compose.production.yml stop
```

### Remove containers:
```powershell
docker-compose -f docker-compose.production.yml down
```

### Update image (jika ada perubahan):
```powershell
docker-compose -f docker-compose.production.yml pull
docker-compose -f docker-compose.production.yml up -d
```


## CATATAN PENTING
## ================================================

1. Pastikan MySQL di 10.100.83.166:3366 sudah running
2. Pastikan database 'finapp' sudah ada
3. Pastikan firewall allow port 8008 dan 8081
4. Pastikan Docker Desktop running di server
5. Default user admin:
   - Username: admin
   - Password: Admin@123


## KONFIGURASI SAAT INI
## ================================================

Frontend:
- Port: 8008
- URL: http://10.100.83.166:8008
- API URL: http://10.100.83.166:8081/api

Backend:
- Port: 8081
- URL: http://10.100.83.166:8081
- Database: 10.100.83.166:3366

MySQL:
- Server: 10.100.83.166
- Port: 3366
- Database: finapp
- User: root
- Password: P@ssw0rd
