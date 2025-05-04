using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public partial class Author
    {
        public int AuthorId { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
