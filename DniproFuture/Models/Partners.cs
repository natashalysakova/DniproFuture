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
    
    public partial class Partners
    {
        public Partners()
        {
            this.PartnersLocalSet = new HashSet<PartnersLocalSet>();
        }
    
        public int Id { get; set; }
        public string Logo { get; set; }
        public string Link { get; set; }
    
        public virtual ICollection<PartnersLocalSet> PartnersLocalSet { get; set; }
    }
}
