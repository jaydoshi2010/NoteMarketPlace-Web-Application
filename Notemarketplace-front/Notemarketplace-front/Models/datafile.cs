using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Notemarketplace_front.Models
{
    public class datafile
    {
        public Database.tblNoteCategory NoteCategory { get; set; }
        public Database.tblUser User { get; set; }
        public Database.tblNoteType NoteType { get; set; }
        public Database.tblCountry Country { get; set; }
    }
}