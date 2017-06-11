using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlBulkLoadExample
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("starting");

            var connectionString = @"Server = ACCESS-1303SF2\SQL2014 ; Database = MockDB ; Trusted_Connection = true";
            var commandText = "SELECT * FROM dbo.MOCK_DATA";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                conn.Open();

                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        using (StreamWriter writer = new StreamWriter(@"c:\temp\file.txt"))
                        {
                            while (reader.Read())
                            {
                                // Using Name and Phone as example columns.
                                var values = new object[reader.FieldCount];
                                reader.GetValues(values);
                                writer.WriteLine(string.Join(",", values.Select(v => v.ToString())));
                            }
                        }
                    }

                }

            }

            Console.WriteLine("done");

            Console.ReadKey();

        }
    }

}