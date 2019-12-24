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
    
    public partial class YTOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public YTOrder()
        {
            this.YTOrderlist = new HashSet<YTOrderlist>();
            this.YTVMLog = new HashSet<YTVMLog>();
        }
    
        public bool Istest { get; set; }
        public System.Guid YTOrderid { get; set; }
        public string YTOrdernumber { get; set; }
        public Nullable<System.Guid> Productid { get; set; }
        public Nullable<System.Guid> Categoryid { get; set; }
        public Nullable<System.Guid> YTMemberid { get; set; }
        public string Url { get; set; }
        public string Service { get; set; }
        public Nullable<int> Count { get; set; }
        public Nullable<int> Remains { get; set; }
        public int YTOrderStatus { get; set; }
        public Nullable<System.DateTime> Duedate { get; set; }
        public Nullable<System.DateTime> Updatedate { get; set; }
        public Nullable<System.DateTime> Createdate { get; set; }
    
        public virtual CategoryMessage CategoryMessage { get; set; }
        public virtual YTMembers YTMembers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<YTOrderlist> YTOrderlist { get; set; }
        public virtual Product Product { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<YTVMLog> YTVMLog { get; set; }
    }
}
