using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    internal class PostgreSQL
    {
        private const string PostgresConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=11299133;Database=postgres";

        InteractionWithInterface interaction = new InteractionWithInterface();

        public void ExportDataToJson(DataGridView dataGrid, string outputPath)
        {
            List<string> selectedColumns = interaction.GetSelectedColumns(dataGrid);

            if (selectedColumns.Count == 0)
            {
                MessageBox.Show("Нет выбранных столбцов.");
                return;
            }

            string query = @"
    WITH executor AS (
        SELECT t.assigned_to AS executor_id,
               e.first_name || ' ' || e.last_name || ', email: ' || e.email AS executor_name
          FROM task t
          JOIN employee e ON t.assigned_to = e.employee_id 
    ),
    link_project AS (
        SELECT t.project_id,
               p.project_name || ' status: ' || p.status AS task_link_project
          FROM task t 
          JOIN project p ON t.project_id = p.project_id
    ),
    leader AS (
        SELECT t.team_id, 
               'leader: ' || e.first_name || ' ' || e.last_name || ', email: ' || e.email AS team_leader
          FROM team t
          JOIN employee e ON t.lead_employee_id = e.employee_id 
    )
    SELECT e.first_name,
           e.last_name,
           e.email,
           p.project_name,
           p.start_date AS project_start_date,
           p.end_date AS project_end_date,
           p.status AS project_status,
           t.task_name,
           ex.executor_name,
           lp.task_link_project,
           t.status AS task_status,
           t.due_date AS task_due_date,
           t2.team_name,
           l.team_leader
      FROM employee e
      LEFT JOIN task t ON t.assigned_to = e.employee_id 
      LEFT JOIN project p ON p.project_id = t.project_id 
      LEFT JOIN executor ex ON ex.executor_id = t.assigned_to
      LEFT JOIN link_project lp ON lp.project_id = t.project_id
      LEFT JOIN project_team pt ON pt.project_id = p.project_id
      LEFT JOIN team t2 ON t2.team_id = pt.team_id
      LEFT JOIN leader l ON l.team_id = t2.team_id";

            string columns = string.Join(", ", selectedColumns);
            string finalQuery = $"SELECT {columns} FROM ({query}) AS subquery;";

            using (var connection = new NpgsqlConnection(PostgresConnectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(finalQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    var dataList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();
                        foreach (string column in selectedColumns)
                        {
                            row[column] = reader[column];
                        }
                        dataList.Add(row);
                    }

                    string jsonOutput = JsonConvert.SerializeObject(dataList, Formatting.Indented);

                    File.WriteAllText(outputPath, jsonOutput);
                }
            }

            MessageBox.Show($"Данные успешно экспортированы в {outputPath}");
        }

        public void Migrate(Library library)
        {
            using (var pgConnection = new NpgsqlConnection(PostgresConnectionString))
            {
                pgConnection.Open();

                // Начало транзакции
                using (var pgTransaction = pgConnection.BeginTransaction())
                {
                    try
                    {
                        // Словари для хранения ID
                        var readerIds = new Dictionary<string, int>();
                        var bookIds = new Dictionary<string, int>();
                        var authorIds = new Dictionary<string, int>();

                        // 1. Вставка или получение ID читателя
                        if (!readerIds.ContainsKey(library.ReaderName))
                        {
                            using (var readerCommand = pgConnection.CreateCommand())
                            {
                                readerCommand.Transaction = pgTransaction;
                                readerCommand.CommandText = @"
                            INSERT INTO readers (name) 
                            VALUES (@name) 
                            ON CONFLICT (name) DO NOTHING 
                            RETURNING reader_id";
                                readerCommand.Parameters.AddWithValue("@name", library.ReaderName);

                                var result = readerCommand.ExecuteScalar();
                                int readerid;
                                if (result != null)
                                {
                                    readerid = (int)result;
                                }
                                else
                                {
                                    readerCommand.CommandText = "SELECT reader_id FROM readers WHERE name = @name";
                                    readerid = (int)readerCommand.ExecuteScalar();
                                }
                                readerIds[library.ReaderName] = readerid;
                            }
                        }

                        int readerId = readerIds[library.ReaderName];

                        // 2. Вставка или получение ID автора
                        if (!authorIds.ContainsKey(library.AuthorName))
                        {
                            using (var authorCommand = pgConnection.CreateCommand())
                            {
                                authorCommand.Transaction = pgTransaction;
                                authorCommand.CommandText = @"
                            INSERT INTO authors (name) 
                            VALUES (@name) 
                            ON CONFLICT (name) DO NOTHING 
                            RETURNING author_id";
                                authorCommand.Parameters.AddWithValue("@name", library.AuthorName);

                                var result = authorCommand.ExecuteScalar();
                                int authorid;
                                if (result != null)
                                {
                                    authorid = (int)result;
                                }
                                else
                                {
                                    authorCommand.CommandText = "SELECT author_id FROM authors WHERE name = @name";
                                    authorid = (int)authorCommand.ExecuteScalar();
                                }
                                authorIds[library.AuthorName] = authorid;
                            }
                        }

                        int authorId = authorIds[library.AuthorName];

                        // 3. Вставка или получение ID книги
                        string bookKey = $"{library.BookTitle}|{library.BookGenre}";
                        if (!bookIds.ContainsKey(bookKey))
                        {
                            using (var bookCommand = pgConnection.CreateCommand())
                            {
                                bookCommand.Transaction = pgTransaction;
                                bookCommand.CommandText = @"
                            INSERT INTO books (title, genre) 
                            VALUES (@title, @genre) 
                            ON CONFLICT (title, genre) DO NOTHING 
                            RETURNING book_id";
                                bookCommand.Parameters.AddWithValue("@title", library.BookTitle);
                                bookCommand.Parameters.AddWithValue("@genre", library.BookGenre);

                                var result = bookCommand.ExecuteScalar();
                                int bookid;
                                if (result != null)
                                {
                                    bookid = (int)result;
                                }
                                else
                                {
                                    bookCommand.CommandText = "SELECT book_id FROM books WHERE title = @title AND genre = @genre";
                                    bookid = (int)bookCommand.ExecuteScalar();
                                }
                                bookIds[bookKey] = bookid;

                                // Связь книги с автором
                                using (var bookAuthorCommand = pgConnection.CreateCommand())
                                {
                                    bookAuthorCommand.Transaction = pgTransaction;
                                    bookAuthorCommand.CommandText = @"
                                INSERT INTO books_authors (book_id, author_id) 
                                VALUES (@book_id, @author_id) 
                                ON CONFLICT (book_id, author_id) DO NOTHING";
                                    bookAuthorCommand.Parameters.AddWithValue("@book_id", bookid);
                                    bookAuthorCommand.Parameters.AddWithValue("@author_id", authorId);

                                    bookAuthorCommand.ExecuteNonQuery();
                                }
                            }
                        }

                        int bookId = bookIds[bookKey];

                        // 4. Вставка записи о выдаче
                        using (var borrowCommand = pgConnection.CreateCommand())
                        {
                            borrowCommand.Transaction = pgTransaction;
                            borrowCommand.CommandText = @"
                        INSERT INTO borrows (book_id, reader_id, borrow_date, return_date) 
                        VALUES (@book_id, @reader_id, @borrow_date, @return_date) 
                        ON CONFLICT (book_id, reader_id, borrow_date) DO NOTHING";
                            borrowCommand.Parameters.AddWithValue("@book_id", bookId);
                            borrowCommand.Parameters.AddWithValue("@reader_id", readerId);
                            borrowCommand.Parameters.AddWithValue("@borrow_date", library.BorrowDate.ToDateTime(new TimeOnly(0, 0)));
                            borrowCommand.Parameters.AddWithValue("@return_date", library.ReturnDate?.ToDateTime(new TimeOnly(0, 0)));

                            borrowCommand.ExecuteNonQuery();
                        }

                        // Фиксация транзакции
                        pgTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Откат транзакции в случае ошибки
                        pgTransaction.Rollback();
                        throw new Exception("Ошибка при миграции данных.", ex);
                    }
                }
            }
        }

        public async void TransferData(Library library)
        {
            using var libraryContext = new LibraryContext();


            var borrow = new Borrow()
            {
                BorrowDate = library.BorrowDate,
                ReturnDate = library.ReturnDate,
            };


            var reader = await libraryContext.Readers.FirstOrDefaultAsync(r => r.Name == library.ReaderName);
            borrow.Reader = reader ?? new Reader() { Name = library.ReaderName };

            libraryContext.Readers.Add(new Reader());

            var book = await libraryContext.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Title == library.BookTitle && b.Genre == library.BookGenre);

            if (book != null)
            {
                borrow.Book = book;

                var author = book.Authors.FirstOrDefault(a => a.Name == library.AuthorName);
                if (author == null)
                {

                    author = await libraryContext.Authors.FirstOrDefaultAsync(a => a.Name == library.AuthorName);
                    if (author == null)
                    {

                        author = new Author() { Name = library.AuthorName };
                        await libraryContext.Authors.AddAsync(author);
                    }

                    book.Authors.Add(author);
                }
            }
            else
            {
                var author = await libraryContext.Authors.FirstOrDefaultAsync(a => a.Name == library.AuthorName);
                if (author == null)
                {
                    author = new Author() { Name = library.AuthorName };
                    await libraryContext.Authors.AddAsync(author);
                }

                borrow.Book = new Book()
                {
                    Title = library.BookTitle,
                    Genre = library.BookGenre,
                    Authors = new List<Author> { author }
                };
            }
            var existingBorrow = await libraryContext.Borrows.FirstOrDefaultAsync(
                a => a.BookId == borrow.Book.BookId &&
                a.ReaderId == borrow.Reader.ReaderId &&
                a.BorrowDate == borrow.BorrowDate &&
                a.ReturnDate == borrow.ReturnDate);
            if (existingBorrow == null)
                libraryContext.Borrows.Add(borrow);

            await libraryContext.SaveChangesAsync();


        }

        public void TruncateTables()
        {
            // Список таблиц для очистки
            string[] tablesToTruncate = {
            "public.employee",
            "public.project",
            "public.project_team",
            "public.task",
            "public.team",
            "public.authors",
            "public.books",
            "public.books_authors",
            "public.readers",
            "public.borrows"
            };

            try
            {
                // Подключение к базе данных
                using (var connection = new NpgsqlConnection(PostgresConnectionString))
                {
                    connection.Open();
                    Console.WriteLine("Подключение к базе данных успешно.");

                    // Создаем команду для выполнения SQL-запросов
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;

                        foreach (var table in tablesToTruncate)
                        {
                            // Формируем SQL-запрос для очистки таблицы
                            string truncateTableQuery = $"TRUNCATE TABLE {table} CASCADE;";
                            command.CommandText = truncateTableQuery;

                            // Выполняем запрос
                            command.ExecuteNonQuery();
                            Console.WriteLine($"Таблица {table} успешно очищена.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                Console.WriteLine($"Ошибка при очистке таблиц: {ex.Message}");
            }
        }
    }
}
