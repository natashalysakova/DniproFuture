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
    
    public partial class NeedHelp
    {
        public NeedHelp()
        {
            this.NeedHelpLocalSet = new HashSet<NeedHelpLocalSet>();
        }
    
        public int Id { get; set; }
        public int Sum { get; set; }
        public int NeedSum { get; set; }
        public string Photos { get; set; }
        public System.DateTime Birthday { get; set; }
        public System.DateTime StartDate { get; set; }
        public bool Done { get; set; }
        public Nullable<System.DateTime> FinishDate { get; set; }
    
        public virtual ICollection<NeedHelpLocalSet> NeedHelpLocalSet { get; set; }
    }
}
