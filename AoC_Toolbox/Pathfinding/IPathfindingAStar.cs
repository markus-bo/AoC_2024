namespace AoC_Toolbox.Pathfinding;

public interface IPathfindingAStar<T> where T : Node
{
    public long searchMinimumPathLength(T startNode, Func<T, long> heuristic, Func<T, bool> pathFoundCondition, long initialLength = 0);
}
