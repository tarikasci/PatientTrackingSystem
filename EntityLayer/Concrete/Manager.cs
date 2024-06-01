using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class Manager
    {
        [Key]
        public int ManagerID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Gender Gender { get; set; }
        public bool ManagerStatus { get; set; }
        public string MailAddress { get; set; }
        public string Password { get; set; }
    }
}