//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CloudControl.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.IGOrder = new HashSet<IGOrder>();
            this.YTMembers = new HashSet<YTMembers>();
            this.YTOrder = new HashSet<YTOrder>();
            this.FBMembers = new HashSet<FBMembers>();
            this.FBOrder = new HashSet<FBOrder>();
            this.FBOrder1 = new HashSet<FBOrder>();
            this.IGMembers = new HashSet<IGMembers>();
        }
    
        public Nullable<System.Guid> Categoryid { get; set; }
        public System.Guid Productid { get; set; }
        public string Productname { get; set; }
        public Nullable<int> BreakTime { get; set; }
        public double Cost { get; set; }
        public double Price { get; set; }
        public int Orders { get; set; }
        public Nullable<System.DateTime> Updatedate { get; set; }
        public Nullable<System.DateTime> Createdate { get; set; }
    
        public virtual CategoryProduct CategoryProduct { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IGOrder> IGOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<YTMembers> YTMembers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<YTOrder> YTOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FBMembers> FBMembers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FBOrder> FBOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FBOrder> FBOrder1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IGMembers> IGMembers { get; set; }
    }
}
