using System;

namespace GithubApi
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var wb = new WebCaller();

            // fetch top repo of orgs
            var repos = wb.FetchRepoAsync("google").GetAwaiter().GetResult();
            repos.Sort((x, y) => y.Forks.CompareTo(x.Forks));

            // fetch top contributors of users
            var topContributors = wb.FetchTopContributorsAsync("google", repos[0].Name).GetAwaiter().GetResult();

           
        }
    }
}
