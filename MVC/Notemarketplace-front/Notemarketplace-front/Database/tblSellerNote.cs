//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Notemarketplace_front.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public partial class tblSellerNote
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblSellerNote()
        {
            this.tblDownloads = new HashSet<tblDownload>();
            this.tblSellerNotesAttachements = new HashSet<tblSellerNotesAttachement>();
            this.tblSellerNotesReportedIssues = new HashSet<tblSellerNotesReportedIssue>();
            this.tblSellerNotesReviews = new HashSet<tblSellerNotesReview>();
        }
    
        public int ID { get; set; }
        public int SellerID { get; set; }
        public int Status { get; set; }
        public Nullable<int> ActionBy { get; set; }
        public string AdminRemarks { get; set; }
        public Nullable<System.DateTime> PublishedDate { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public int Category { get; set; }
        public string DisplayPicture { get; set; }
        public Nullable<int> NoteType { get; set; }
        public Nullable<int> NumberofPages { get; set; }
        [Required]
        public string Description { get; set; }
        public string UniversityName { get; set; }
        public Nullable<int> Country { get; set; }
        public string Course { get; set; }
        public string CourseCode { get; set; }
        public string Professor { get; set; }
        public bool IsPaid { get; set; }
        public Nullable<decimal> SellingPrice { get; set; }
        
        public string NotesPreview { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModificationDate { get; set; }
        public Nullable<int> ModificationBy { get; set; }
        public bool IsActive { get; set; }


        public HttpPostedFileBase uploadNote { get; set; }
        public HttpPostedFileBase displayPic { get; set; }
        public HttpPostedFileBase notePreview { get; set; }

        public virtual tblCountry tblCountry { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblDownload> tblDownloads { get; set; }
        public virtual tblNoteCategory tblNoteCategory { get; set; }
        public virtual tblNoteType tblNoteType { get; set; }
        public virtual tblReferenceData tblReferenceData { get; set; }
        public virtual tblUser tblUser { get; set; }
        public virtual tblUser tblUser1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblSellerNotesAttachement> tblSellerNotesAttachements { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblSellerNotesReportedIssue> tblSellerNotesReportedIssues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblSellerNotesReview> tblSellerNotesReviews { get; set; }
    }
}
