using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DniproFuture.Models
{
    public class NeedHelpMetadata
    {
        [Range(0, int.MaxValue)]
        public int Sum { get; set; }

        [Range(1, int.MaxValue)]
        public int NeedSum { get; set; }
        public string Photos { get; set; }

        [DataType(DataType.DateTime)]
        public System.DateTime Birthday { get; set; }

        [DataType(DataType.DateTime)]
        public System.DateTime StartDate { get; set; }

        public bool Done { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? FinishDate { get; set; }
    }

    public class NeedHelpLocalSetMetadata
    {
        [StringLength(32)]
        [Required(ErrorMessage = "Поле \"Фамилия\" не может быть пустым")]
        public string LastName { get; set; }

        [StringLength(32)]
        [Required(ErrorMessage = "Поле \"Имя\" не может быть пустым")]
        public string FirstName { get; set; }

        [StringLength(32)]
        public string Patronymic { get; set; }

        [StringLength(64)]
        [Required(ErrorMessage = "Поле \"Адрес\" не может быть пустым")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Поле \"Информация\" не может быть пустым")]
        public string About { get; set; }
    }

    public class NewsMetadata
    {

    }

    public class NewsLocalSetMetadata
    {
        [Required(ErrorMessage = "Поле \"Заголовок\" не может быть пустым")]
        [StringLength(128)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Поле \"Текст\" не может быть пустым")]
        public string Text { get; set; }
    }

    public class PartnersMetadata
    {

    }

    public class PartnersLocalSetMetadata
    {
        [StringLength(32)]
        [Required(ErrorMessage = "Поле \"Имя\" не может быть пустым")]
        public string Name { get; set; }
    }

    public class ProjectsMetadata
    {
        [Range(0, int.MaxValue)]
        public int Sum { get; set; }

        [Range(1, int.MaxValue)]
        public int NeedSum { get; set; }
        public string Photos { get; set; }

        [DataType(DataType.DateTime)]
        public System.DateTime StartDate { get; set; }

        public bool Done { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? FinishDate { get; set; }
    }

    public class ProjectsLocalSetMetadata
    {
        [StringLength(32)]
        [Required(ErrorMessage = "Поле \"Имя\" не может быть пустым")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Поле \"Информация\" не может быть пустым")]
        public string AboutProject { get; set; }
    }
}