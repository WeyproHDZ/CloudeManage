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
    
    public partial class FBMembers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FBMembers()
        {
            this.FBMembersLoginlog = new HashSet<FBMembersLoginlog>();
            this.FBOrderlist = new HashSet<FBOrderlist>();
            this.FBOrder = new HashSet<FBOrder>();
        }
    
        public Nullable<int> Isenable { get; set; }
        public int Isdocker { get; set; }
        public int Isnew { get; set; }
        public Nullable<double> AccountCost { get; set; }
        public System.Guid FBMemberid { get; set; }
        public Nullable<System.Guid> Productid { get; set; }
        public string FB_Account { get; set; }
        public string FB_Password { get; set; }
        public string FB_Name { get; set; }
        public string Useragent { get; set; }
        public string UserDataUrl { get; set; }
        public string Facebooklink { get; set; }
        public string Cookie { get; set; }
        public Nullable<long> Lastdate { get; set; }
        public Nullable<System.DateTime> Updatedate { get; set; }
        public Nullable<System.DateTime> Createdate { get; set; }
        public string Mega_Account { get; set; }
        public string Mega_Password { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FBMembersLoginlog> FBMembersLoginlog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FBOrderlist> FBOrderlist { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FBOrder> FBOrder { get; set; }
        public virtual Product Product { get; set; }
    }
}
