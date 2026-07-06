using MySqlConnector;
using OrderMedia.DigiKam.Interfaces;
using OrderMedia.DigiKam.Models;

namespace OrderMedia.DigiKam.Repositories;

public class MariaDbRepository : IRepository
{
    private readonly MySqlDataSource _dataSource;

    public MariaDbRepository(MySqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<Tag?> GetTagByTagNameAsync(string tagName, CancellationToken cancellationToken = default)
    {
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = connection.CreateCommand();

        command.CommandText = $"""
                               SELECT 
                                   id,
                                   name
                               FROM Tags
                               WHERE name = @tagName
                               """;

        command.Parameters.AddWithValue("@tagName", tagName);
        
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        if (await reader.ReadAsync(cancellationToken))
        {
            return new Tag(
                reader.GetInt32(0),
                reader.GetString(1));
        }

        return null;
        
    }

    public async Task<IEnumerable<string>> GetMediaPathsAsync(GetMediaPathsQuery query, CancellationToken cancellationToken = default)
    {
        var result = new List<string>();
        var conditions = new List<string>();
        
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = connection.CreateCommand();

        command.Parameters.AddWithValue("@format", query.Format.ToString());
        conditions.Add($"ii.format = @format");
        conditions.Add("i.status = 1");
        
        for (var i = 0; i < query.IncludingTags.Count; i++)
        {
            var parameterName = $"@includingTagId_{i}";
            command.Parameters.AddWithValue(parameterName, query.IncludingTags[i].Id);
            conditions.Add($"EXISTS (SELECT 1 FROM ImageTags it WHERE it.imageid = i.id AND it.tagid = {parameterName})");
        }
        
        for (var i = 0; i < query.ExcludingTags.Count; i++)
        {
            var parameterName = $"@excludingTagId_{i}";
            command.Parameters.AddWithValue(parameterName, query.ExcludingTags[i].Id);
            conditions.Add($"NOT EXISTS (SELECT 1 FROM ImageTags it WHERE it.imageid = i.id AND it.tagid = {parameterName})");
        }
        
        command.CommandText = $"""
                                SELECT
                                    CONCAT(a.relativePath, '/', i.name) as path
                                FROM Images i
                                         JOIN Albums a ON i.album = a.id
                                         JOIN ImageTags it ON it.imageid = i.id
                                         JOIN ImageInformation ii ON ii.imageid = i.id
                                WHERE {string.Join(" AND ", conditions)}
                               """;
        
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            result.Add(reader.GetString(0));
        }
        
        return result;
    }
}