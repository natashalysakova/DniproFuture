//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DniproFuture.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class NeedHelpLocal
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string About { get; set; }
        public int NeedHelpId { get; set; }
        public int LanguageId { get; set; }
        public string Patronymic { get; set; }
    
        public virtual NeedHelp NeedHelp { get; set; }
        public virtual Language Language { get; set; }
    }
}
