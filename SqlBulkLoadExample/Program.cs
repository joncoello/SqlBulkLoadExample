using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
            var queryCommandText = "SELECT * FROM dbo.MOCK_DATA";
            var truncateCommandText = "truncate table dbo.MOCK_DATA";
            var bulkInsertCommandText = @"BULK INSERT dbo.MOCK_DATA FROM 'C:\temp\bulkdata.txt' WITH ( FIELDTERMINATOR = ',', ROWTERMINATOR = '\n' )";

            Stopwatch sw;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                conn.Open();

                sw = Stopwatch.StartNew();

                using (SqlCommand cmd = new SqlCommand(queryCommandText, conn))
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

                sw.Stop();
                Console.WriteLine("query took " + sw.Elapsed.ToString());

                sw = Stopwatch.StartNew();

                using (var cmd = new SqlCommand(truncateCommandText, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                sw.Stop();
                Console.WriteLine("truncate took " + sw.Elapsed.ToString());

                sw = Stopwatch.StartNew();

                using (var cmd = new SqlCommand(bulkInsertCommandText, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                sw.Stop();
                Console.WriteLine("insert took " + sw.Elapsed.ToString());

            }

            Console.WriteLine("done");

            Console.ReadKey();

        }
    }

}