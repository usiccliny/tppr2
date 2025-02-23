using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;

namespace WinFormsApp1
{
    internal class InteractionWithInterface
    {
        readonly string connectionString = "Data Source=mydatabase.db;Version=3;";

        public void SetDataGrid(DataGridView dataGrid)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "PRAGMA table_info(project)";
                using (SQLiteCommand cmd = new SQLiteCommand(selectQuery, connection))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    DataTable dataTable = new DataTable();

                    dataTable.Columns.Add("Column", typeof(string));
                    dataTable.Columns.Add("Upload", typeof(bool));

                    while (reader.Read())
                    {
                        string columnName = reader["name"].ToString();
                        dataTable.Rows.Add(columnName, false);
                    }

                    dataGrid.DataSource = dataTable;
                }
            }
        }

        public List<string> GetSelectedColumns(DataGridView dataGrid)
        {
            List<string> selectedColumns = new List<string>();
            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Upload"].Value))
                {
                    selectedColumns.Add(row.Cells["Column"].Value.ToString());
                }
            }
            return selectedColumns;
        }
    }
}
