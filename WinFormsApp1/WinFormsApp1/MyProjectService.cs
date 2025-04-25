using Grpc.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using WinFormsApp1.Protos;

namespace WinFormsApp1
{
    public class MyProjectService : ProjectService.ProjectServiceBase
    {
        public override async Task<ProjectResponse> CreateProject(IAsyncStreamReader<ProjectRequest> requestStream, ServerCallContext context)
        {
            var receivedProjects = new List<ProjectRequest>();

            try
            {
                PostgreSQL postgre = new PostgreSQL();
                while (await requestStream.MoveNext())
                {
                    var project = requestStream.Current;
                    receivedProjects.Add(project);

                    var saveproject = new Project(project);
                    postgre.Migrate(saveproject);

                    MessageBox.Show($"Received project: {project.ProjectName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while processing projects: {ex.Message}");
                return new ProjectResponse
                {
                    Message = "An error occurred while processing projects."
                };
            }

            MessageBox.Show($"Total projects received: {receivedProjects.Count}");

            return new ProjectResponse
            {
                Message = $"Successfully processed {receivedProjects.Count} projects."
            };
        }
    }
}