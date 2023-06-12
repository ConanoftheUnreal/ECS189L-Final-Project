public class LevelGenerator
{

    // A level generator must be initialized with a LevelLayoutGenerator and an IRoomGenerator
    private LevelLayoutGenerator _layoutGenerator;
    private IRoomGenerator _roomGenerator;

    // The entrance to a room from its parent always has the exit ID 0
    public static int ENTRANCE_EXIT_ID = 0;

    public LevelGenerator(LevelLayoutGenerator layoutGenerator, IRoomGenerator roomGenerator)
    {

        _layoutGenerator = layoutGenerator;
        _roomGenerator = roomGenerator;

    }

    public Room Generate()
    {

        // Generate the layout of the level
        LevelLayoutNode rootNode = _layoutGenerator.Generate();
        // And then generate all of the rooms for each node in that layout
        ExpandNode(rootNode);

        // Then return the room at the root of that layout
        return rootNode.room;

    }

    private void ExpandNode(LevelLayoutNode node)
    {
        
        // Generate a room for the current node
        if (node.type != NodeType.BOSS)
        {
            node.SetRoom(_roomGenerator.Generate(node.children.Count + 1));
        }
        else
        {
            // Generate an exit from the BOSS room that basically leaves the level
            node.SetRoom(_roomGenerator.Generate(node.children.Count + 2));
        }

        if (node.type == NodeType.ROOT)
        {

            // If this is the root room, we want to attach a LevelManager component to it            
            LevelManager levelManager = node.room.roomObject.AddComponent<LevelManager>();
            levelManager.SetCurrentNode(node);
            // And name it "Root"
            node.room.roomObject.name = "Root";
            node.room.roomObject.tag = "Root";

        }
        else 
        {

            // Otherwise, set the parent of the room the room of the parent node of this node, and then disable the room
            node.room.roomObject.transform.SetParent(node.parent.room.roomObject.transform);
            // Only spawn enemies at generation time if this isn't the root room
            node.room.SpawnEnemies();
            node.room.DisableRoom();

        }

        // For each child of this current node
        for (int i = 0; i < node.children.Count; i++)
        {

            // Expand the child node
            LevelLayoutNode child = node.children[i];
            ExpandNode(child);

            // And then name it depending on its ID relative to its parent and whether or not it is the BOSS room
            if (child.type == NodeType.NORMAL)
            {
                child.room.roomObject.name = "Child #" + i.ToString();
            }
            else if (child.type == NodeType.BOSS)
            {
                child.room.roomObject.name = "(BOSS) Child #" + i.ToString();
            }

        }

    }

}