namespace Golf.Scoreboard.Api.Model;

public class Scoreboard
{
    public string last_updated { get; set; }
    public string last_updated_str { get; set; }
    public string data_source { get; set; }
    public IEnumerable<Player> players { get; set; } = new List<Player>();

}
