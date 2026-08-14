using System.Collections.Generic;

public class nan_DialogueDatabase
{
    public Dictionary<int, nan_DialogueNode> nodes = new Dictionary<int, nan_DialogueNode>();

    public void AddNode(nan_DialogueNode node)
    {
        if (!nodes.ContainsKey(node.ID))
        {
            nodes.Add(node.ID, node);
        }
    }

    public nan_DialogueNode GetNode(int id)
    {
        if (nodes.ContainsKey(id))
        {
            return nodes[id];
        }

        return null;
    }
}