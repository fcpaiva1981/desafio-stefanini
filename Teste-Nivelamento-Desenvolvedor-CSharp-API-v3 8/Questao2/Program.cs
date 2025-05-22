using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class Program
{
    static async Task Main(string[] args)
    {
        await ShowTeamGoals("Paris Saint-Germain", 2013);
        await ShowTeamGoals("Chelsea", 2014);
    }

    static async Task ShowTeamGoals(string team, int year)
    {
        int totalGoals = await GetTeamGoals(team, year);
        Console.WriteLine($"Team {team} scored {totalGoals} goals in {year}");
    }

    static async Task<int> GetTeamGoals(string team, int year)
    {
        int totalGoals = 0;
        totalGoals += await GetGoalsForTeam(team, year, "team1", "team1goals");
        totalGoals += await GetGoalsForTeam(team, year, "team2", "team2goals");
        return totalGoals;
    }

    static async Task<int> GetGoalsForTeam(string team, int year, string teamParam, string goalsParam)
    {
        int goals = 0;
        int page = 1;
        bool hasMore = true;
        using (HttpClient client = new HttpClient())
        {
            while (hasMore)
            {
                string url =
                    $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&{teamParam}={team}&page={page}";
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                    {
                        var root = doc.RootElement;
                        var data = root.GetProperty("data");
                        foreach (var match in data.EnumerateArray())
                        {
                            goals += int.Parse(match.GetProperty(goalsParam).GetString());
                        }

                        int totalPages = root.GetProperty("total_pages").GetInt32();
                        page++;
                        if (page > totalPages) hasMore = false;
                    }
                }
                else
                {
                    hasMore = false;
                }
            }
        }

        return goals;
    }
}