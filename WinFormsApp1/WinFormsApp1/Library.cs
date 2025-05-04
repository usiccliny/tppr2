using Protos;

namespace WinFormsApp1
{
    public partial class Library
    {
        public int Id { get; set; }

        public string BookTitle { get; set; } = null!;

        public string BookGenre { get; set; } = null!;

        public string AuthorName { get; set; } = null!;

        public string ReaderName { get; set; } = null!;

        public DateOnly BorrowDate { get; set; }

        public DateOnly? ReturnDate { get; set; }

        public override string ToString() =>
            $"{BookTitle} {AuthorName} {ReaderName} {BorrowDate}";

        public Library() { }

        public Library(LibraryRequest request)
        {
            BookTitle = request.BookTitle;
            BookGenre = request.BookGenre;
            AuthorName = request.AuthorName;
            ReaderName = request.ReaderName;
            BorrowDate = DateOnly.FromDateTime(DateTimeOffset.FromUnixTimeSeconds(request.BorrowDate).UtcDateTime);
            if (request.ReturnDate != 0)
            {
                ReturnDate = DateOnly.FromDateTime(DateTimeOffset.FromUnixTimeSeconds(request.ReturnDate).UtcDateTime);
            }
            else
            {
                ReturnDate = null;
            }
        }
    }
}
