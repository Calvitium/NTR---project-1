using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P01.Models
{
    public class Note
    {
        public String Title {get;set;}
        public DateTime ValidityDate {get;set;}
        public String Category {get;set;}
        public int uniqueID {get;}
        public int displayID {get; set;}
        public String Content {get;set;}

        public string submitMode;

        public Note(string submit = "edit")
        {
            Title = "New";
            Content = "Your text";
            ValidityDate = DateTime.Now;
            Category = "any";
            submitMode = submit;
            uniqueID = Notebook.currentNoteID++;

        }    
    }
}