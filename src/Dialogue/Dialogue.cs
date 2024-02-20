public class Dialogue
{
    public Dictionary<string, Node> Nodes = new Dictionary<string, Node>();
    public Node? CurrentNode = null;
    public Dialogue(){}
    
    /// <summary>
    /// Add a node to the dialogue.
    /// </summary>
    /// <param name="Id">Id of the node.</param>
    /// <param name="Text">The dialogue text.</param>
    /// <param name="Options">A list of dialogue options.</param>
    public void AddNode(string Id, string Text, List<Option> Options)
    {
        this.Nodes.Add(Id, new Node(Text, Options));
        if (this.CurrentNode == null)
        {
            this.CurrentNode = this.Nodes[Id];
        }
    }

    /// <summary>
    /// Step to the next option.
    /// </summary>
    /// <param name="Choice">The index of the choice.</param>
    /// return true=stop false=keep going
    public bool Step(int Choice)
    {
        if (CurrentNode.Options.Count == 0)
        {
            // there is no next option
            return true;
        }
        if (Choice < 0 || Choice >= CurrentNode.Options.Count) { return false; }
        string NextNodeId = CurrentNode.Options[Choice].NextNode;
        if (CurrentNode.Options[Choice].ResetNode != null && Nodes.ContainsKey(CurrentNode.Options[Choice].ResetNode))
        {
            this.CurrentNode = Nodes[CurrentNode.Options[Choice].ResetNode];
            return true;
        }
        else if (Nodes.ContainsKey(NextNodeId))
        {
            this.CurrentNode = Nodes[NextNodeId];
        }
        return false;
    }

    /// <summary>
    /// Returns a list of keys with choice text.
    /// </summary>
    public Dictionary<int, string> GetChoices()
    {
        Dictionary<int, string> Choices = new Dictionary<int, string>();
        for (int i = 0; i < CurrentNode.Options.Count; i++)
        {
            Choices.Add(i, CurrentNode.Options[i].Text);
        }
        return Choices;
    }

}