using UnityEngine;
using System.Collections.Generic;

public enum NodeType
{

    ROOT,
    NORMAL,
    BOSS

}

public class LevelLayoutNode
{

    private Room _room;

    private LevelLayoutNode _parent;
    private List<LevelLayoutNode> _children = new List<LevelLayoutNode>();
    private int _depth;
    private NodeType _type;

    public LevelLayoutNode(int depth, NodeType type)
    {

        _depth = depth;
        _type = type;
        
    }

    public int depth
    {
        get
        {
            return _depth;
        }
    }

    public NodeType type
    {
        get
        {
            return _type;
        }
    }

    public void SetType(NodeType type)
    {
        _type = type;
    }

    public LevelLayoutNode parent
    {
        get
        {
            return _parent;
        }
    }

    public void SetParent(LevelLayoutNode node)
    {
        _parent = node;
    }

    public List<LevelLayoutNode> children
    {
        get
        {
            return _children;
        }
    }

    public Room room
    {
        get
        {
            return _room;
        }
    }

    public void SetRoom(Room room)
    {
        _room = room;
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

        LevelLayoutNode root = new LevelLayoutNode(1, NodeType.ROOT);
        _numRooms++;

        LevelLayoutNode currentNode = root;
        int currentDepth = 1;

        // Make the path to the boss
        while (currentDepth < _maxDepth)
        {

            AddChild(currentNode);
            currentDepth++;
            currentNode = currentNode.children[0];

            if (currentDepth == _maxDepth)
            {
                currentNode.SetType(NodeType.BOSS);
            }

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

        if ((_numRooms < _maxRooms) && (parent.children.Count < _maxChildren) && (parent.depth < _maxDepth))
        {

            LevelLayoutNode child = new LevelLayoutNode(parent.depth + 1, NodeType.NORMAL);
            child.SetParent(parent);
            parent.children.Add(child);
            _numRooms++;

        }

    }

}