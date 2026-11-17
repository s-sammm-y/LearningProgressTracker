using System.Text;

namespace LearningTracker.Services.Helpers;

public static class CsvExporter
{
    public static byte[] Export<T>(
        IEnumerable<T> records,
        params (string Header, Func<T, string?> Value)[] columns)
    {
        var csvContent = new StringBuilder();

        csvContent.AppendLine(
            string.Join(",", columns.Select(c => Escape(c.Header))));

        foreach (var record in records)
        {
            var values = columns.Select(column =>
                Escape(column.Value(record)));

            csvContent.AppendLine(string.Join(",", values));
        }

        var encoding = new UTF8Encoding(true);

        return encoding.GetPreamble()
            .Concat(encoding.GetBytes(csvContent.ToString()))
            .ToArray();
    }

    private static string Escape(string? value)
    {
        value ??= string.Empty;

        return $"\"{value.Replace("\"", "\"\"")}\"";
    }
}