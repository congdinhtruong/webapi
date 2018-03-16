//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApi.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    
    public partial class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Book()
        {
            this.RentalDetails = new HashSet<RentalDetail>();
        }
    
        public int CallNumber { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public Nullable<int> AuthorId { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string Photo { get; set; }
        public Nullable<int> Views { get; set; }
        
        [JsonIgnore]
        public virtual Author Author { get; set; }
        [JsonIgnore]
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<RentalDetail> RentalDetails { get; set; }
    }
}