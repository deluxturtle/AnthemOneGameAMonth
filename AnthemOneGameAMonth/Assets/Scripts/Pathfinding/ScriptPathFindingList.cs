using UnityEngine;
using System.Collections.Generic;

public class ScriptPathFindingList {

    public List<ScriptNodeRecord> list;

    public ScriptPathFindingList()
    {
        list = new List<ScriptNodeRecord>();
    }

    public ScriptNodeRecord smallestElement()
    {
        ScriptNodeRecord smallest = list[0];
        foreach(ScriptNodeRecord node in list)
        {
            if(node.costSoFar < smallest.costSoFar)
            {
                smallest = node;
            }
        }

        return smallest;
    }
}
