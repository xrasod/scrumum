//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Scrum.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Country
    {
        public Country()
        {
            this.TravelInfoes = new HashSet<TravelInfo>();
        }
    
        public int CID { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public int Subsistence { get; set; }
    
        public virtual ICollection<TravelInfo> TravelInfoes { get; set; }
    }
}
