using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public partial class Book
    {
        public int BookId { get; set; }

        public string Title { get; set; } = null!;

        public string Genre { get; set; } = null!;

        public virtual ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();

        public virtual ICollection<Author> Authors { get; set; } = new List<Author>();
    }
}
