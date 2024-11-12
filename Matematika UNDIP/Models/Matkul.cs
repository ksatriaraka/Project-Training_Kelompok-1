namespace Matematika_UNDIP.Models
{
    public class Matkul
    {
        public string Kode { get; set; }
        public string NamaMataKuliah { get; set; }
        public List<Dosen> DosenPengampu { get; set; }
    }
}
