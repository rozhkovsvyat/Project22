using System.Data.SqlClient;
using System.Configuration;

var conStr = ConfigurationManager.ConnectionStrings[nameof(SqlConnection)]?.ConnectionString ?? 
             throw new InvalidOperationException($"Connection string {nameof(SqlConnection)} not found.");

using var con = new SqlConnection(conStr);

con.StateChange += (s, _) =>
{
	Console.ForegroundColor = ConsoleColor.Green;
	Console.WriteLine(string.Concat(nameof(SqlConnection), " is ", ((SqlConnection)s).State.ToString().ToLower(), '.'));
	Console.ForegroundColor = ConsoleColor.White;
};

try
{
	con.Open();
	using var cmd = new SqlCommand(ConfigurationManager.AppSettings[nameof(SqlCommand)] ?? 
				throw new InvalidOperationException($"Command {nameof(SqlCommand)} not found."), con);

	var r = cmd.ExecuteReader();

	Console.BackgroundColor = ConsoleColor.White;
	Console.ForegroundColor = ConsoleColor.Black;
	Console.WriteLine("\n{0, 3} | {1, 35} | {2, 13} | {3, 7}", "#", "Tea", "Category", "Type ");
	Console.BackgroundColor = ConsoleColor.Black;
	Console.ForegroundColor = ConsoleColor.White;

	while (r.Read()) Console.WriteLine($"{r[0],3} | " +
	                                   $"{r[1],35} | " +
	                                   $"{r[2],13} | " +
	                                   $"{r[3],7} ");
	Console.WriteLine();
	con.Close();
}

catch (Exception e) { Console.WriteLine(e.Message); }

finally
{
	Console.ForegroundColor = ConsoleColor.DarkGray;
	Console.WriteLine("Press any key to exit...");
	Console.ReadKey();
}
