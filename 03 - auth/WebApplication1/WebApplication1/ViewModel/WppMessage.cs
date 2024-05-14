namespace WebApplication1.ViewModel;

public class WppMessage
{
    public string instance_id { get; set; }
    public string instance_token { get; set; }
    public List<string> message { get; set; }
    public List<string> phone{ get; set; }
}