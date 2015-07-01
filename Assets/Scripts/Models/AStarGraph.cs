using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Models
{
    // in this for every free position is 4 nodes (for each direction). Droid can go forward (or cannot if there is 
    // obstacle). Or it can rotate left or right. Adjacency matrix in such case is unefficient so adjacency list is used
    public class AStarGraph
    {
        private Vertex[,,] vertices;  
        private int mapLength;
        private int mapWidth;

        private const float rotateWeight = 0.3f;
        private const float moveWeight = 1f;

        public AStarGraph(int[,] map)
        {
            mapLength = map.GetUpperBound(0) + 1;
            mapWidth = map.GetUpperBound(1) + 1;

            vertices = new Vertex[mapLength,mapWidth,4];

            // init vertices for the all free positions
            for (var i = 0; i < mapLength; i++)
            {
                for (var j = 0; j < mapWidth; j++)
                {
                    // if free
                    if (map[i, j] == 0)
                    {
                        // create vertices for direction 0 +z, 1 +x, 2 -z, 3 -x
                        var _0 = new Vertex { position = new MatrixPosition { x = i, z = j, k = 0 } };
                        var _1 = new Vertex { position = new MatrixPosition {x = i, z = j, k = 1} };
                        var _2 = new Vertex { position = new MatrixPosition {x = i, z = j, k = 2} };
                        var _3 = new Vertex { position = new MatrixPosition {x = i, z = j, k = 3} };
                        // add them to list
                        vertices[i, j, 0] = _0;
                        vertices[i, j, 1] = _1;
                        vertices[i, j, 2] = _2;
                        vertices[i, j, 3] = _3;
                        // connect them with the each other
                        var edge01 = new Edge { From = _0, To = _1, Weight = rotateWeight };
                        var edge03 = new Edge { From = _0, To = _3, Weight = rotateWeight };
                        _0.Output.Add(edge01);
                        _0.Output.Add(edge03);
                        var edge12 = new Edge { From = _1, To = _2, Weight = rotateWeight };
                        var edge10 = new Edge { From = _1, To = _0, Weight = rotateWeight };
                        _1.Output.Add(edge12);
                        _1.Output.Add(edge10);
                        var edge21 = new Edge { From = _2, To = _1, Weight = rotateWeight };
                        var edge23 = new Edge { From = _2, To = _3, Weight = rotateWeight };
                        _2.Output.Add(edge21);
                        _2.Output.Add(edge23);
                        var edge30 = new Edge { From = _3, To = _0, Weight = rotateWeight };
                        var edge32 = new Edge { From = _3, To = _2, Weight = rotateWeight };
                        _3.Output.Add(edge30);
                        _3.Output.Add(edge32);
                    }
                }
            }
            // connect rotations with the neighbours
            for (var i = 0; i < mapLength; i++)
            {
                for (var j = 0; j < mapWidth; j++)
                {
                    // check is there any vertice exists
                    if (vertices[i, j, 0] != null)
                    {
                        if (i != mapLength - 1 && vertices[i + 1, j, 1] != null)
                        {
                            var edge = new Edge
                            {
                                From = vertices[i, j, 1],
                                To = vertices[i + 1, j, 1],
                                Weight = moveWeight
                            };
                            vertices[i, j, 1].Output.Add(edge);
                        }
                        if (i != 0 && vertices[i - 1, j, 3] != null)
                        {
                            var edge = new Edge
                            {
                                From = vertices[i, j, 3],
                                To = vertices[i - 1, j, 3],
                                Weight = moveWeight
                            };
                            vertices[i, j, 3].Output.Add(edge);
                        }
                        if (j != mapWidth - 1 && vertices[i, j + 1, 0] != null)
                        {
                            var edge = new Edge
                            {
                                From = vertices[i, j, 0],
                                To = vertices[i, j+1, 0],
                                Weight = moveWeight
                            };
                            vertices[i, j, 0].Output.Add(edge);
                        }
                        if (j != 0 && vertices[i, j - 1, 2] != null)
                        {
                            var edge = new Edge
                            {
                                From = vertices[i, j, 2],
                                To = vertices[i, j - 1, 2],
                                Weight = moveWeight
                            };
                            vertices[i, j, 2].Output.Add(edge);
                        }
                    }
                }
            }
        }

        // find the best path with the AStar
        public Path GeneratePath(MatrixPosition start, MatrixPosition end)
        {
            var pathes = new List<Path>();
            var startVertex = vertices[start.x, start.z, start.k];
            var first = new Path {frontier = startVertex};
            pathes.Add(first);
            return Find(pathes, end);
        }

        private Path Find(List<Path> pathes, MatrixPosition end)
        {
            // find the path with cheaper frontier
            var orderedPathes = pathes.OrderBy(p => p.Cost(end.x, end.z));
            var current = orderedPathes.First();
            if (current.frontier.position.x == end.x && current.frontier.position.z == end.z)
            {
                return current;
            }
            // explore current frontier
            foreach (var v in current.frontier.Output)
            {
                if (!CheckVertexExplored(v.To, pathes))
                {
                    var newPath = new Path { actions = new List<Edge>(current.actions) { v }, frontier = v.To };
                    pathes.Add(newPath);
                }
            }
            // remove current, it is not needed more
            pathes.Remove(current);
            return Find(pathes, end);
        }
        private bool CheckVertexExplored(Vertex vertex, List<Path> pathes)
        {
            foreach (var p in pathes)
            {
                if (p.frontier == vertex)
                {
                    return true;
                }
                foreach (var a in p.actions)
                {
                    if (a.From == vertex)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public class Vertex
    {
        public float Data;
        public MatrixPosition position;

        public List<Edge> Output = new List<Edge>();
    }

    public class Edge
    {
        public Vertex To;
        public Vertex From;

        public float Weight;
    }

    public class Path
    {
        public Vertex frontier;
        public List<Edge> actions = new List<Edge>();

        public float Cost(int x, int z)
        {
            var cost = Mathf.Sqrt(Mathf.Pow(frontier.position.x - x, 2) + Mathf.Pow(frontier.position.z - z, 2));
            // assume that first edge in path always lead to 
            foreach (var a in actions)
            {
                cost += a.Weight;
            }
            return cost;
        }
    }
}
