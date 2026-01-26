using System;

namespace FinApp.Domain.Entities
{
    public class AnggaranMaster : BaseEntity
    {
        public int TahunAnggaran { get; set; }
        public int Revisi { get; set; }
        public string? KdDept { get; set; }
        public string? NmDept { get; set; }
        public string? KdUnit { get; set; }
        public string? NmUnit { get; set; }
        public string? KdDekon { get; set; }
        public string? NmDekon { get; set; }
        public string? KdSatker { get; set; }
        public string? NmSatker { get; set; }
        public string? KdFungsi { get; set; }
        public string? NmFungsi { get; set; }
        public string? KdSFung { get; set; }
        public string? NmSFung { get; set; }
        public string? KdProgram { get; set; }
        public string? NmProgram { get; set; }
        public string? KdGiat { get; set; }
        public string? NmGiat { get; set; }
        public string? KdOutput { get; set; }
        public string? NmOutput { get; set; }
        public string? KdSOutput { get; set; }
        public string? NmSOutput { get; set; }
        public string? KdKmpnen { get; set; }
        public string? NmKmpnen { get; set; }
        public string? KdSkmpnen { get; set; }
        public string? NmSkmpnen { get; set; }
        public string? KdAkun { get; set; }
        public string? NmAkun { get; set; }
        public string? Bkpk { get; set; }
        public string? KdSDana { get; set; }
        public string? NmSDana { get; set; }
        public string? KdBeban { get; set; }
        public string? Register { get; set; }
        public string? NoItem { get; set; }
        public string? NmItem { get; set; }
        public string? VolKeg { get; set; }
        public string? SatKeg { get; set; }
        public decimal? HargaSat { get; set; }
        public string? KomponenPendukung { get; set; }
        public decimal? Pagu { get; set; }
        public decimal? HasilReviuKonsolidasiBaru { get; set; }
        public decimal? Netto { get; set; }
    }
}
