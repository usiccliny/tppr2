using Grpc.Core;
using Protos;
using System.Collections.Generic;
using System.Threading.Tasks;
using WinFormsApp1.Protos;

namespace WinFormsApp1
{
    public class MyLibraryService : LibraryService.LibraryServiceBase
    {
        public override async Task<LibraryResponse> CreateLibrary(IAsyncStreamReader<LibraryRequest> requestStream, ServerCallContext context)
        {
            var receivedLibraries = new List<LibraryRequest>();

            try
            {
                PostgreSQL postgre = new PostgreSQL();
                while (await requestStream.MoveNext())
                {
                    var library = requestStream.Current;
                    receivedLibraries.Add(library);

                    var saveLibrary = new Library(library);
                    //postgre.Migrate(saveLibrary);
                    postgre.TransferData(saveLibrary);

                    MessageBox.Show($"Received project: {library.AuthorName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while processing projects: {ex.Message}");
                return new LibraryResponse
                {
                    Message = "An error occurred while processing projects."
                };
            }

            MessageBox.Show($"Total projects received: {receivedLibraries.Count}");

            return new LibraryResponse
            {
                Message = $"Successfully processed {receivedLibraries.Count} projects."
            };
        }
    }
}