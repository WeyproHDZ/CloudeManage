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
    
    public partial class CategoryMessage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CategoryMessage()
        {
            this.Message = new HashSet<Message>();
            this.IGOrder = new HashSet<IGOrder>();
            this.YTOrder = new HashSet<YTOrder>();
            this.FBOrder = new HashSet<FBOrder>();
        }
    
        public System.Guid Categoryid { get; set; }
        public string CategoryName { get; set; }
        public Nullable<System.DateTime> Updatedate { get; set; }
        public Nullable<System.DateTime> Createdate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message> Message { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IGOrder> IGOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<YTOrder> YTOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FBOrder> FBOrder { get; set; }
    }
}