using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P01.Models
{
    public static class NotesFilter
    {
        public static String category {get;set;}
        
        public static DateTime fromDate {get;set;}

        public static DateTime toDate {get;set;}
        
        static NotesFilter(){
            fromDate = DateTime.Now.AddDays(-1);
            toDate = DateTime.Now.AddYears(1);
            category = "any";
        }
        public static void updateFilter(DateTime from, DateTime to, string Category){
            category = Category;
            fromDate = from;
            toDate = to;
        }
    }
}