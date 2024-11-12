using System.ComponentModel.DataAnnotations;

namespace Matematika_UNDIP.Models
{
    public class Dosen
    {
        public string DosenID { get; set; }
        public string Name { get; set; }
        public string NIP { get; set; }
        public string Telephone { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public bool Active { get; set; }
    }
}
