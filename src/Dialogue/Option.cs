public class Option
{
    public string Text;
    public string? NextNode;
    public string? ResetNode = null;
    public Option(string Text, string NextNode=null, string ResetNode=null)
    {
        this.Text = Text;
        this.NextNode = NextNode;
        this.ResetNode = ResetNode;
    }
}