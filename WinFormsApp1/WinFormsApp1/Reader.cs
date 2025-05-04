using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public partial class Reader
    {
        public int ReaderId { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();
    }
}
