using System.ComponentModel.DataAnnotations;

namespace DniproFuture.Models.OutputModels
{
    public class ContactsOutputModel
    {
        
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Message { get; set; }
    }
}
