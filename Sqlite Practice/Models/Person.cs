using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sqlite_Practice.Models
{
   public class Person
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Age { get; set; }
        [MaxLength(50)]
        public String Name { get; set; }
    }
}
