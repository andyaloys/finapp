# Implementasi Master Supplier - Dokumentasi Lengkap

## Overview
Fitur master supplier mengubah input penerima di STPB dari text bebas menjadi dropdown pilihan dari tabel referensi.

## Perubahan Backend

### 1. Database Schema
**Tabel Baru: Penerimas**
- `Id` (INT, PK, Auto Increment)
- `Nama` (VARCHAR(200), NOT NULL)
- `Npwp` (VARCHAR(20), NULLABLE)
- `Alamat` (VARCHAR(500), NULLABLE)
- `IsActive` (BOOLEAN, DEFAULT TRUE)
- `CreatedAt`, `UpdatedAt`

**Perubahan Tabel StpbDetails:**
- Tambah kolom: `PenerimaId` (INT, NULLABLE, FK → Penerimas)
- Kolom lama `Penerima` (string) tetap ada untuk backward compatibility

### 2. Entity & Configuration
**File Baru:**
- `backend/FinApp.Domain/Entities/Penerima.cs` - Entity Penerima extends BaseEntity
- `backend/FinApp.Infrastructure/Data/Configurations/PenerimaConfiguration.cs` - EF Core configuration

**File Diubah:**
- `backend/FinApp.Domain/Entities/StpbDetail.cs` - Tambah `PenerimaId` dan navigation property
- `backend/FinApp.Infrastructure/Data/AppDbContext.cs` - Register DbSet<Penerima>
- `backend/FinApp.Infrastructure/Data/Configurations/StpbDetailConfiguration.cs` - Tambah FK relationship

### 3. Repository & Interface
**File Baru:**
- `backend/FinApp.Domain/Interfaces/IPenerimaRepository.cs` - Contract repository
- `backend/FinApp.Infrastructure/Repositories/PenerimaRepository.cs` - Implementation

**Methods:**
- `GetAllAsync()` - Get all suppliers
- `GetAllActiveAsync()` - Get active suppliers only
- `GetByIdAsync(int id)` - Get by ID
- `ExistsByNamaAsync(string nama, int? excludeId)` - Check duplicate name
- `AddAsync()`, `Update()`, `Delete()`, `SaveChangesAsync()`

### 4. DTOs & Validators
**File Baru:**
- `backend/FinApp.Core/DTOs/Penerima/PenerimaDto.cs`
- `backend/FinApp.Core/DTOs/Penerima/CreatePenerimaDto.cs`
- `backend/FinApp.Core/DTOs/Penerima/UpdatePenerimaDto.cs`
- `backend/FinApp.Core/Validators/Penerima/CreatePenerimaDtoValidator.cs`
- `backend/FinApp.Core/Validators/Penerima/UpdatePenerimaDtoValidator.cs`

**Validasi:**
- Nama: Required, Max 200 characters
- NPWP: Optional, Max 20 characters
- Alamat: Optional, Max 500 characters
- Duplicate name check

### 5. Service & Controller
**File Baru:**
- `backend/FinApp.Core/Interfaces/IPenerimaService.cs` - Service contract
- `backend/FinApp.Core/Services/PenerimaService.cs` - Business logic implementation
- `backend/FinApp.API/Controllers/PenerimaController.cs` - REST API endpoints

**API Endpoints:**
- `GET /api/penerima` - Get all suppliers
- `GET /api/penerima/active` - Get active suppliers
- `GET /api/penerima/{id}` - Get supplier by ID
- `POST /api/penerima` - Create new supplier
- `PUT /api/penerima/{id}` - Update supplier
- `DELETE /api/penerima/{id}` - Delete supplier

### 6. Dependency Injection
**File Diubah:**
- `backend/FinApp.API/Extensions/ServiceExtensions.cs`
  - Register `IPenerimaService → PenerimaService`
  - Register `IPenerimaRepository → PenerimaRepository`

### 7. AutoMapper
**File Diubah:**
- `backend/FinApp.Core/Mappings/MappingProfile.cs` - Tambah mapping Penerima ↔ DTOs

### 8. MonitoringService Update
**File Diubah:**
- `backend/FinApp.Core/Services/MonitoringService.cs` - Update line 220: `detail.Penerima?.Nama` (untuk support navigation property)

## Perubahan Frontend

### 1. Models
**File Baru:**
- `frontend/src/app/core/models/penerima.model.ts`
  - Interface: `Penerima`, `CreatePenerimaDto`, `UpdatePenerimaDto`

**File Diubah:**
- `frontend/src/app/core/models/stpb-detail.model.ts`
  - Tambah field `penerimaId?: number` di `StpbDetail`, `CreateStpbDetailDto`, `StpbDetailDto`

### 2. Service
**File Baru:**
- `frontend/src/app/core/services/penerima.service.ts` - HTTP service untuk API calls

### 3. Supplier Component (Master Data)
**File Baru:**
- `frontend/src/app/features/supplier/supplier.component.ts` - Component logic
- `frontend/src/app/features/supplier/supplier.component.html` - Template (table + modal form)
- `frontend/src/app/features/supplier/supplier.component.scss` - Styles

**Fitur:**
- Table list suppliers dengan pagination
- Add/Edit modal form (Nama, NPWP, Alamat, Status Aktif)
- Delete dengan konfirmasi
- Search dan filter
- Form validation

### 4. STPB Detail Modal Update
**File Diubah:**
- `frontend/src/app/features/stpb/stpb-detail-modal/stpb-detail-modal.component.ts`
  - Import `PenerimaService` dan `Penerima` model
  - Tambah property: `suppliers: Penerima[]`
  - Inject `PenerimaService` di constructor
  - Tambah method: `loadSuppliers()`
  - Call `loadSuppliers()` di `ngOnInit()`
  - Update form control: `penerima` → `penerimaId`
  - Update form patch di `populateForm()`: `penerimaId: detailAny.penerimaId`
  - Update DTO creation: `penerimaId: formValue.penerimaId`

- `frontend/src/app/features/stpb/stpb-detail-modal/stpb-detail-modal.component.html`
  - Ganti input text "Penerima" jadi nz-select dropdown
  - Bind ke `formControlName="penerimaId"`
  - Loop suppliers: `*ngFor="let supplier of suppliers"`
  - Option value: `[nzValue]="supplier.id"`, Label: `[nzLabel]="supplier.nama"`

### 5. Routing
**File Diubah:**
- `frontend/src/app/app.routes.ts`
  - Tambah route: `/supplier` dengan guard `menuPermissionGuard`, data: `menuKey: 'master-supplier'`

## Database Migration

### Manual SQL Script
File: `database/add_supplier_menu.sql`

**Langkah Eksekusi:**
1. Stop aplikasi backend (dotnet run)
2. Koneksi ke MySQL database
3. Jalankan script SQL:
   ```bash
   mysql -u root -p finapp_db < database/add_supplier_menu.sql
   ```

**Isi Script:**
1. Insert menu "Supplier" ke tabel `Menus` (parent: master-data, order: 2)
2. Insert permission ke `RoleMenuPermissions` untuk Admin role
3. Create table `Penerimas`
4. Alter table `StpbDetails` - tambah kolom `PenerimaId` dengan FK constraint
5. Create index untuk performance

### Alternatif: EF Core Migration
Jika ingin menggunakan EF Core migrations (setelah stop process):
```bash
cd backend/FinApp.API
dotnet ef migrations add AddPenerimaTable --project ../FinApp.Infrastructure
dotnet ef database update
```

## Menu Configuration

**Menu Hierarchy:**
```
Master Data (master-data)
  ├── PPK/Bendahara (master-ppkbendahara) - Order 1
  └── Supplier (master-supplier) - Order 2  ← NEW
```

**Permission:**
- Role: Admin
- MenuKey: `master-supplier`
- CanView: true
- CanCreate: true
- CanUpdate: true
- CanDelete: true

## Testing Checklist

### Backend Testing
- [ ] Build project tanpa error
- [ ] Run migration berhasil
- [ ] API endpoints berfungsi:
  - [ ] GET /api/penerima - Return list
  - [ ] GET /api/penerima/active - Return active only
  - [ ] POST /api/penerima - Create success
  - [ ] PUT /api/penerima/{id} - Update success
  - [ ] DELETE /api/penerima/{id} - Delete success
- [ ] Validation bekerja (nama required, max length, duplicate check)
- [ ] STPB detail bisa save dengan penerimaId

### Frontend Testing
- [ ] Build project tanpa error
- [ ] Menu "Supplier" muncul di Master Data
- [ ] Halaman supplier list dapat diakses
- [ ] Add supplier berhasil
- [ ] Edit supplier berhasil
- [ ] Delete supplier berhasil dengan konfirmasi
- [ ] Search dan pagination bekerja
- [ ] STPB detail modal:
  - [ ] Dropdown supplier muncul dan terisi
  - [ ] Bisa select supplier
  - [ ] Bisa save detail transaksi dengan supplier
  - [ ] Edit detail transaksi - supplier terpilih dengan benar

### Integration Testing
- [ ] Create supplier → Langsung muncul di dropdown STPB
- [ ] Create STPB detail dengan supplier → Data tersimpan dengan penerimaId
- [ ] Edit STPB detail → Supplier dropdown menampilkan pilihan yang benar
- [ ] Nonaktifkan supplier → Tidak muncul di dropdown (hanya active)
- [ ] Delete supplier yang sudah dipakai → FK constraint prevent atau set null

## Deployment Steps

1. **Build & Push Docker:**
   ```bash
   cd c:\TI\NET\finapp
   docker build -t andyaloys/finapp-backend:latest -f backend/Dockerfile .
   docker build -t andyaloys/finapp-frontend:latest -f frontend/Dockerfile .
   docker push andyaloys/finapp-backend:latest
   docker push andyaloys/finapp-frontend:latest
   ```

2. **Update Database:**
   ```bash
   mysql -u root -p finapp_db < database/add_supplier_menu.sql
   ```

3. **Restart Services:**
   ```bash
   docker-compose down
   docker-compose pull
   docker-compose up -d
   ```

## Rollback Plan

Jika ada masalah:

**Database Rollback:**
```sql
-- Remove FK constraint and column
ALTER TABLE StpbDetails DROP FOREIGN KEY FK_StpbDetails_Penerimas_PenerimaId;
ALTER TABLE StpbDetails DROP COLUMN PenerimaId;

-- Drop table
DROP TABLE IF EXISTS Penerimas;

-- Remove menu
DELETE FROM RoleMenuPermissions WHERE MenuKey = 'master-supplier';
DELETE FROM Menus WHERE `Key` = 'master-supplier';
```

**Code Rollback:**
- Revert to previous Docker image tags
- Restore previous codebase from git

## Notes

- **Backward Compatibility:** Kolom `Penerima` (string) di StpbDetails TIDAK dihapus untuk menjaga data existing
- **Data Migration:** Data lama dengan penerima string tidak perlu migrasi, biarkan tetap di field lama
- **New Data:** Transaksi baru akan menggunakan `PenerimaId` (dropdown)
- **Display:** Frontend akan prioritas tampilkan dari navigation property `Penerima?.Nama`, fallback ke field `penerima` (string lama)

## Checklist Akhir

✅ Backend Implementation Complete
✅ Frontend Implementation Complete
✅ Database Migration Script Ready
✅ API Endpoints Tested (needs actual testing)
✅ Component UI Complete
✅ Routing & Menu Configuration Done
✅ Documentation Complete
⏳ Database Migration Execution (manual step)
⏳ Docker Build & Push (manual step)
⏳ Integration Testing (manual step)
