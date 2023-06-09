using UnityEngine;
using System.Collections.Generic;

// Each node is either the ROOT, the BOSS node, or just a NORMAL node
public enum NodeType
{

    ROOT,
    NORMAL,
    BOSS

}

public class LevelLayoutNode
{

    // The Room will be generated for the node and given to it by the LevelGenerator
    private Room _room;

    // Nodes basically form a tree structure
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

    // LevelLayoutGenerator must be parameterized with these values
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

        // Keep track of the number of nodes added to the tree
        _numRooms = 0;

        // Create the first node
        LevelLayoutNode root = new LevelLayoutNode(1, NodeType.ROOT);
        _numRooms++;

        LevelLayoutNode currentNode = root;
        int currentDepth = 1;

        // Make the path to the boss by just adding nodes in a straight line until you reach depth maxDepth
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

        // Now, expand the root node with a random number of children, taking into account that it is guaranteed to already have one child
        ExpandNode(root, Random.Range(0, _maxChildren));

        return root;

    }

    private void ExpandNode(LevelLayoutNode node, int numChildren)
    {
        
        // Add the child nodes to this node
        for (int i = 0; i < numChildren; i++)
        {
            AddChild(node);
        }

        // And then expand each child node with a random number of children
        foreach (LevelLayoutNode child in node.children)
        {
            ExpandNode(child, Random.Range(0, _maxChildren + 1));
        }

    }

    private void AddChild(LevelLayoutNode parent)
    {

        // Only add a child to a parent if we have not reached max rooms in level, max depth, or max children for the parent
        if ((_numRooms < _maxRooms) && (parent.children.Count < _maxChildren) && (parent.depth < _maxDepth))
        {

            // Create a new NORMAL node and add it as a child of its parent
            LevelLayoutNode child = new LevelLayoutNode(parent.depth + 1, NodeType.NORMAL);
            child.SetParent(parent);
            parent.children.Add(child);
            _numRooms++;

        }

    }

}