using Golf.Scoreboard.Api.Model;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/scoreboard", ([FromServices] IMemoryCache memoryCache) =>
{
    var cachedScoredboard = memoryCache.Get<Scoreboard>("scoreboard");

    if(cachedScoredboard is not null)
    {
        return cachedScoredboard;
    }

    var url = "https://www.espn.com/golf/leaderboard";
    var web = new HtmlWeb();
    var doc = web.Load(url);
    
    var playerNodes = doc.DocumentNode.Descendants("tbody")
        .First().ChildNodes
        .Where(n => n.Name == "tr" && !n.Attributes["class"].Value.Contains("cutline"));

    var playerRows = playerNodes.Select(playerRow =>
    {

        var player = new Player();

        if (playerRow.ChildNodes.Count == 3) //pre-masters
        {
            player.player = playerRow.ChildNodes[1].ChildNodes[1].InnerText;
            player.country_flag_image = playerRow.ChildNodes[1].ChildNodes[0].Attributes["src"].Value;
            player.link = playerRow.ChildNodes[1].ChildNodes[1].Attributes["href"].Value;
            player.thru = playerRow.ChildNodes[2].InnerText;
        }
        else
        {
            player.pos = playerRow.ChildNodes[1].InnerText;
            player.player = playerRow.ChildNodes[2].ChildNodes[1].InnerText;
            player.country_flag_image = playerRow.ChildNodes[2].ChildNodes[0].Attributes["src"].Value;
            player.link = playerRow.ChildNodes[2].ChildNodes[1].Attributes["href"].Value;
            player.to_par = playerRow.ChildNodes[3].InnerText;
            player.today = playerRow.ChildNodes[4].InnerText;
            player.thru = playerRow.ChildNodes[5].InnerText;
            player.r1 = playerRow.ChildNodes[6].InnerText;
            player.r2 = playerRow.ChildNodes[7].InnerText;
            player.r3 = playerRow.ChildNodes[8].InnerText;
            player.r4 = playerRow.ChildNodes[9].InnerText;
            player.tot = playerRow.ChildNodes[10].InnerText;
        }

        return player;
    });

    var utcNow = DateTimeOffset.UtcNow;

    long timeNow = utcNow.ToUnixTimeSeconds();
    string timeStamp = timeNow.ToString();

    string isoTimestamp = utcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

    var scoreboard = new Scoreboard
    {
        last_updated = timeStamp,
        last_updated_str = isoTimestamp,
        data_source = url,
        players = playerRows.ToArray()
    };

    memoryCache.Set("scoreboard", scoreboard, TimeSpan.FromSeconds(30));

    return scoreboard;
})
.WithName("GetScoreboard");

app.Run();
