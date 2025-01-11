using System.Text.Json;

public class Program
{
    public static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }


    public static async Task<int> getTotalScoredGoals(string team, int year)
    {
        HttpClient client = new HttpClient();
        var totalGoals = 0;
        var currentPage = 1;
        var totalPages = 3;

        while (currentPage <= totalPages)
        {
            var apiResult = await client.GetStreamAsync(
                $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page={currentPage}"
            );

            var result = await JsonSerializer.DeserializeAsync<Matches>(apiResult);

            totalGoals += result.data.Sum(x => Int32.Parse(x.team1goals));

            currentPage++;
        }


        currentPage = 1;

        while (currentPage <= totalPages)
        {
           var apiResult = await client.GetStreamAsync(
                $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}&page={currentPage}"
            );

            var result = await JsonSerializer.DeserializeAsync<Matches>(apiResult);

            totalGoals += result.data.Sum(x => Int32.Parse(x.team2goals));

            currentPage++;
        }


        return totalGoals;
    }

    class Matches
    {
        public int page { get; set; }

        public int per_page { get; set; }

        public int total { get; set; }

        public int total_pages { get; set; }

        public Competition[] data { get; set; }
    }

    class Competition
    {
        public string competition { get; set; }

        public int year { get; set; }

        public string round { get; set; }

        public string team1 { get; set; }

        public string team2 { get; set; }

        public string team1goals { get; set; }

        public string team2goals { get; set; }
    }

}