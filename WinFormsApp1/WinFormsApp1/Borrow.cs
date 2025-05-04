using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public partial class Borrow
    {
        public int BorrowId { get; set; }

        public int? BookId { get; set; }

        public int? ReaderId { get; set; }

        public DateOnly BorrowDate { get; set; }

        public DateOnly? ReturnDate { get; set; }

        public virtual Book? Book { get; set; }

        public virtual Reader? Reader { get; set; }
    }
}
