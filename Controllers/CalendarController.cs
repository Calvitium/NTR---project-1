using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.IO;
using System.Text.Encodings.Web;
using System.Collections.Generic;
using P01.Models;


namespace P01.Controllers
{
    public class CalendarController : Controller
    {
        // 
        // GET: /Calendar/

       public IActionResult Index() => View();

       public IActionResult Edit(int ID) => View(Notebook.DisplayedNotes.Find(note => note.uniqueID == ID));
       public IActionResult NextPage()
        {
            if( Notebook.recentlyTouchedNote/10 * 10 + 10 < Notebook.DisplayedNotes.Count )
                Notebook.recentlyTouchedNote = Notebook.recentlyTouchedNote/10 * 10 + 10;
            return View("Index");
        }
        public IActionResult PreviousPage()
        {
            if( Notebook.recentlyTouchedNote - 10 >= 0)
                Notebook.recentlyTouchedNote = Notebook.recentlyTouchedNote - 10;
            return View("Index");
        }

        public IActionResult New()
        {   
            Note newNote = new Note("create");
            Notebook.AllNotes.Add(newNote);
            return  View("Edit", newNote);
        }
        
        public IActionResult SubmitChanges(int id, String title, string text, DateTime date, string category)
        {
            Note modifiedNote = Notebook.AllNotes.Find(note => note.uniqueID == id);
            string path;
            bool titleAlreadyUsed() => modifiedNote.submitMode == "create" && Notebook.AllNotes.Find(note => note.Title == title && note.uniqueID != id) != null;
            if(modifiedNote.submitMode == "edit")
                removeFile(modifiedNote.Title);
            while(titleAlreadyUsed())
            {
                modifiedNote.Title += " - copy";
                title += " - copy";
            }
            modifiedNote.Title = title;
            modifiedNote.Category = category;
            modifiedNote.Content = text;
            modifiedNote.ValidityDate = date;
            modifiedNote.submitMode = "edit";
            Notebook.updateDisplay(id);

            path = Path.Combine(Notebook.DirectoryName, modifiedNote.Title);
            text = "category:"+category+'\n'+"date:"+date.ToString("MM/dd/yyyy")+'\n'+text;
            System.IO.File.WriteAllText(path,text);
             
            return View("Index");
                        
        }
        public IActionResult DeleteFile(int id)
        {
            Note modifiedNote = Notebook.AllNotes.Find(note => note.uniqueID == id);
            removeFile(modifiedNote.Title);
            Notebook.AllNotes.Remove(modifiedNote);
            Notebook.updateDisplay(-1);
            return View("Index");
        }
 
        public IActionResult Filter(DateTime from, DateTime to, string category)
        { 
           NotesFilter.updateFilter(from, to, category);
           Notebook.updateDisplay(-1);
           return View("Index");
        }
        
        void removeFile(string title) {
            string path = Path.Combine(Notebook.DirectoryName,title);
            System.IO.File.Delete(path);
        }
    }
}