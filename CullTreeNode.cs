using System;

public class CullTreeNode
{
    private int id;
    //private CullTreeNode rootNode;
    private CullTreeNode left { get; set; }
    private CullTreeNode right { get; set; }

    public CullTreeNode(int id)
	{
        this.id = id;
        //this.rootNode = rootNode;
        this.left = null;
        this.right = null;
	}
}
