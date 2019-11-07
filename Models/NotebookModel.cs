using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using P01.Models;

namespace P01.Models
{
    public static class Notebook
    {
        public static List<Note> AllNotes {get;set;}
        public static List<Note> DisplayedNotes {get;set;}
        public static int currentNoteID {get;set;}
        public static int recentlyTouchedNote {get;set;}
        public static string DirectoryName {get;set;}


        static Notebook()
        {
            AllNotes = new List<Note>();
            DirectoryName = Environment.CurrentDirectory + "/Notes";
            initializeNotes();
            updateDisplay(-1);
        }    

        static void initializeNotes()
        {
            string[] fileNames = Directory.GetFiles(DirectoryName);
            StreamReader file;
            Note newNote;
            string categoryHeader, dateHeader, textLine;
            int day, month, year;
            foreach(string name in fileNames)
            {
                newNote = new Note();
                file = new StreamReader(name);
                checkFormat();
                if(isFormatProper())
                    {
                        fillNote(name); 
                        AllNotes.Add(newNote);
                        file.Close();
                    }  
            }




            AllNotes = AllNotes.OrderBy(note => note.ValidityDate.Day).ToList();

            void checkFormat(){
                categoryHeader = file.ReadLine();
                dateHeader = file.ReadLine();
            }

            bool isFormatProper() => categoryHeader.Substring(0,9)=="category:" && dateHeader.Substring(0,5)=="date:";

            void fillNote(string fileName){
                year = Int32.Parse(dateHeader.Substring(11));
                month = Int32.Parse(dateHeader.Substring(5,2));
                day = Int32.Parse(dateHeader.Substring(8,2));
                newNote.ValidityDate = new DateTime(year, month, day);
                newNote.Category = categoryHeader.Substring(9);
                newNote.Title = fileName.Substring(fileName.LastIndexOf('/')+1);
                newNote.Content = "";
                while((textLine = file.ReadLine())!=null)
                    newNote.Content += textLine;
            }
        }

        public static void updateDisplay(int ID = 0){
            DisplayedNotes = new List<Note>(AllNotes).OrderBy(note => note.ValidityDate).ToList();
            DisplayedNotes = NotesFilter.category == "any" ? 
                DisplayedNotes.FindAll(note => 
                                                            note.ValidityDate >= NotesFilter.fromDate &&
                                                            note.ValidityDate <= NotesFilter.toDate)
            :
               DisplayedNotes.FindAll(note =>  note.Category     == NotesFilter.category &&   
               note.ValidityDate >= NotesFilter.fromDate  &&  note.ValidityDate <= NotesFilter.toDate);

            for(int i = 0; i<DisplayedNotes.Count; i++)
                DisplayedNotes[i].displayID = i;
            Notebook.recentlyTouchedNote = ID == -1? 0 : Notebook.DisplayedNotes.Find(note => note.uniqueID == ID).displayID;
        }
    }
    
}   