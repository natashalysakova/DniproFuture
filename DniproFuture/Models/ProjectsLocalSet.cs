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
    
    public partial class ProjectsLocalSet
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string AboutProject { get; set; }
        public int ProjectId { get; set; }
        public int LanguageId { get; set; }
    
        public virtual Language Language { get; set; }
        public virtual Projects Projects { get; set; }
    }
}
