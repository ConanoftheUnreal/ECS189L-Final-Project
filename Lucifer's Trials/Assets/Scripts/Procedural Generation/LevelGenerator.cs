public class LevelGenerator
{

    private LevelLayoutGenerator _layoutGenerator;
    private IRoomGenerator _roomGenerator;

    public static int ENTRANCE_EXIT_ID = 0;

    public LevelGenerator(LevelLayoutGenerator layoutGenerator, IRoomGenerator roomGenerator)
    {

        _layoutGenerator = layoutGenerator;
        _roomGenerator = roomGenerator;

    }

    public Room Generate()
    {

        LevelLayoutNode rootNode = _layoutGenerator.Generate();
        ExpandNode(rootNode);

        return rootNode.room;

    }

    private void ExpandNode(LevelLayoutNode node)
    {
        
        node.SetRoom(_roomGenerator.Generate(node.children.Count + 1));

        if (node.type == NodeType.ROOT)
        {
            
            LevelManager levelManager = node.room.roomObject.AddComponent<LevelManager>();
            levelManager.SetCurrentNode(node);

        }

        //node.room.OpenExit(ENTRANCE_EXIT_ID);
        node.room.OpenAllExits();

        if (node.type != NodeType.ROOT)
        {

            node.room.roomObject.transform.SetParent(node.parent.room.roomObject.transform);
            node.room.DisableRoom();
            
        }
        else 
        {
            node.room.roomObject.name = "Root";
        }

        for (int i = 0; i < node.children.Count; i++)
        {

            LevelLayoutNode child = node.children[i];
            ExpandNode(child);

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