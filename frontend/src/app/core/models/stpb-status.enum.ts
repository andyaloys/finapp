export enum StpbStatus {
  Draft = 0,
  Kirim = 1,
  Approve = 2,
  Dikembalikan = 3
}

export function getStatusDisplay(status: StpbStatus): string {
  switch (status) {
    case StpbStatus.Draft:
      return 'Draft';
    case StpbStatus.Kirim:
      return 'Dikirim';
    case StpbStatus.Approve:
      return 'Disetujui';
    case StpbStatus.Dikembalikan:
      return 'Dikembalikan';
    default:
      return 'Unknown';
  }
}

export function getStatusClass(status: StpbStatus): string {
  switch (status) {
    case StpbStatus.Draft:
      return 'badge-secondary';
    case StpbStatus.Kirim:
      return 'badge-info';
    case StpbStatus.Approve:
      return 'badge-success';
    case StpbStatus.Dikembalikan:
      return 'badge-warning';
    default:
      return 'badge-secondary';
  }
}
