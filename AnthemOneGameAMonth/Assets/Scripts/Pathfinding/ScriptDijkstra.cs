using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class ScriptDijkstra {
    
    public List<GameObject> PathFindDijkstra(GameObject start, GameObject goal, Human human)
    {
        //initialize first node
        ScriptNodeRecord startRecord = new ScriptNodeRecord();
        startRecord.node = start;
        startRecord.connection = null;
        startRecord.costSoFar = 0;

        //initialize the open and closed lists
        ScriptPathFindingList open = new ScriptPathFindingList();
        ScriptPathFindingList closed = new ScriptPathFindingList();
        open.list.Add(startRecord);


        ScriptNodeRecord current = new ScriptNodeRecord();
        //iterate through processing each node
        
        while (open.list.Count > 0)
        {
            //find the smallest element
            current = open.smallestElement();
            //if it is the goal node, then we found the goal.
            if (current.node == goal)
                break;
            //get its outgoing connections
            List<ScriptConnection> connections = current.node.GetComponent<Tile>().Connections;


            foreach (ScriptConnection connection in connections)
            {
                ScriptNodeRecord endNode = new ScriptNodeRecord();
                endNode.node = connection.goingTo;
                
                if (!endNode.node.GetComponent<Tile>().IsOccupied() || endNode.node.GetComponent<Tile>().occupiedBy.Faction == human.Faction)
                {
                    //get the cost estimate for the end node
                    endNode.connection = current;
                    int endNodeCost = current.costSoFar + connection.cost;


                    //if this node is already closed get outa this connection
                    if (closed.list.Contains(endNode))
                    {
                        continue;
                    }
                    else if (open.list.Contains(endNode))
                    {
                        //here we find the record in the open list
                        //corresponding to the endNode
                        ScriptNodeRecord endNodeRecord = open.list.Find(item => item == endNode);
                        if (endNodeRecord.costSoFar <= endNodeCost)
                        {
                            continue;
                        }
                        //Otherwise we know we've got an unvisited
                        //node, so make a record for it

                    }
                    else
                    {
                        ScriptNodeRecord newEndNode = new ScriptNodeRecord();
                        newEndNode.node = endNode.node;
                        newEndNode.costSoFar = endNodeCost;
                        newEndNode.connection = current;
                        if (!open.list.Exists(item => item.node == newEndNode.node))
                        {
                            open.list.Add(newEndNode);
                        }
                    }

                }
            }

            open.list.Remove(current);
            closed.list.Add(current);
        }


        if (current.node != goal)
        {
            //we've run out of nodes without finding the goal, so there's no
            //solution.
            return null;
        }
        else
        {
            List<GameObject> path = new List<GameObject>();
            
            while (current.node != start)
            {
                path.Add(current.node);
                current = current.connection;
            }
            path.Reverse();
            return path;
        }
    }
}
