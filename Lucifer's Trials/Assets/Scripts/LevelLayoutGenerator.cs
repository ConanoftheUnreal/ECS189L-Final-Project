using UnityEngine;
using System.Collections.Generic;

public class LevelLayoutNode
{

    public LevelLayoutNode parent;
    public List<LevelLayoutNode> children = new List<LevelLayoutNode>();

    private int _depth;

    public LevelLayoutNode(int depth)
    {
        _depth = depth;
    }

    public int getDepth()
    {
        return _depth;
    }

}

public class LevelLayoutGenerator
{

    private int _maxDepth;
    private int _maxRooms;
    private int _maxChildren;

    private int _numRooms;

    public LevelLayoutGenerator(int maxDepth, int maxRooms, int maxChildren)
    {

        _maxDepth = maxDepth;
        _maxRooms = maxRooms;
        _maxChildren = maxChildren;

    }

    public LevelLayoutNode Generate()
    {

        _numRooms = 0;

        LevelLayoutNode root = new LevelLayoutNode(1);
        _numRooms++;

        LevelLayoutNode currentNode = root;

        int currentDepth = 1;

        // Make the path to the boss
        while (currentDepth < _maxDepth)
        {

            AddChild(currentNode);
            currentDepth++;
            currentNode = currentNode.children[0];

        }

        ExpandNode(root, Random.Range(0, _maxChildren));

        return root;

    }

    private void ExpandNode(LevelLayoutNode node, int numChildren)
    {
        
        for (int i = 0; i < numChildren; i++)
        {
            AddChild(node);
        }

        foreach (LevelLayoutNode child in node.children)
        {
            ExpandNode(child, Random.Range(0, _maxChildren + 1));
        }

    }

    private void AddChild(LevelLayoutNode parent)
    {

        if ((_numRooms < _maxRooms) && (parent.children.Count < _maxChildren) && (parent.getDepth() < _maxDepth))
        {

            LevelLayoutNode child = new LevelLayoutNode(parent.getDepth() + 1);
            child.parent = parent;
            parent.children.Add(child);
            _numRooms++;

        }

    }

}