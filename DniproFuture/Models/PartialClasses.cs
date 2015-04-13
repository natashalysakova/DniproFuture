using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DniproFuture.Models
{
    [MetadataType(typeof(NeedHelpMetadata))]
    public partial class NeedHelp : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Sum > NeedSum)
                yield return new ValidationResult("Cумма не должна быть больше необходимой", new[] { "Sum", "NeedSum" });

            if (Birthday > DateTime.Now)
                yield return new ValidationResult("Дата рождения не должна быть из будущего", new[] { "Birthday" });

            if (StartDate > FinishDate)
                yield return new ValidationResult("Дата начала сбора должна быть раньше даты окончания", new[] { "FinishDate", "StartDate" });
        }
    }


    [MetadataType(typeof(NeedHelpLocalSetMetadata))]
    public partial class NeedHelpLocalSet
    {}


    [MetadataType(typeof(NewsMetadata))]
    public partial class News
    {
    }

    [MetadataType(typeof(NewsLocalSetMetadata))]
    public partial class NewsLocalSet
    { }


    [MetadataType(typeof(PartnersLocalSetMetadata))]
    public partial class PartnersLocalSet
    { }
    [MetadataType(typeof(PartnersMetadata))]
    public partial class Partners
    {
    }

    [MetadataType(typeof(ProjectsMetadata))]
    public partial class Projects : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Sum > NeedSum)
                yield return new ValidationResult("Cумма не должна быть больше необходимой", new[] { "Sum", "NeedSum" });

            if (StartDate > FinishDate)
                yield return new ValidationResult("Дата начала сбора должна быть раньше даты окончания", new[] { "FinishDate", "StartDate" });
        }
    }
    [MetadataType(typeof(ProjectsLocalSetMetadata))]
    public partial class ProjectsLocalSet
    { }



}