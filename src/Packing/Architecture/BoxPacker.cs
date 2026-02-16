namespace EvolutionaryContainerPacking.Packing.Architecture;

using EvolutionaryContainerPacking.Packing.Architecture.Boxes;
using EvolutionaryContainerPacking.Packing.Architecture.Containers;
using EvolutionaryContainerPacking.Packing.Architecture.Geometry;
using EvolutionaryContainerPacking.Packing.Architecture.Placements;

/// <summary>
/// Packs pending boxes into containers using placement heuristics and container constraints.
/// Manages multiple opened containers and handles rotations automatically if needed.
/// </summary>
public class BoxPacker
{
    private readonly ContainerProperties _containerProperties;

    private readonly List<OpenedContainer> _containers;

    /// <summary>
    /// Provides data of all opened containers.
    /// </summary>
    public IReadOnlyList<ContainerData> ContainersData => (from cont in _containers select cont.Data).ToList().AsReadOnly();

    /// <summary>
    /// Initializes a BoxPacker with specified container properties.
    /// </summary>
    public BoxPacker(ContainerProperties containerProperties)
    {
        _containerProperties = containerProperties;
        _containers = new List<OpenedContainer>();
    }

    /// <summary>
    /// Packs multiple pending boxes into containers sequentially.
    /// </summary>
    /// <param name="boxes">Boxes to be packed.</param>
    /// <returns>Data of all containers after packing.</returns>
    public IReadOnlyList<ContainerData> PackBoxes(IEnumerable<PendingBox> boxes)
    {
        foreach (PendingBox box in boxes)
        {
            PackBox(box);
        }
        return ContainersData;
    }

    /// <summary>
    /// Attempts to pack a single pending box into the available containers.
    /// <para>
    /// The method follows a sequence of steps:
    /// 1. Tries to pack the box with its current rotation using its placement heuristic.
    /// 2. If no valid placement is found:
    ///    a) Checks if the box weight exceeds the container's max weight and throws if so.
    ///    b) Tries all possible rotations to see if the box can fit in any container.
    /// 3. If the box still cannot be packed, a new container is added.
    /// 4. After adding a new container, it retries packing with the original and in case of failure even with rotated boxes.
    /// 5. If no placement is possible even after rotation and a new container, an exception is thrown indicating that the box is too large.
    /// </para>
    /// This ensures that boxes are only placed if they respect container constraints (size and weight),
    /// and automatically handles creating additional containers and rotation adjustments.
    /// </summary>
    /// <param name="box">The PendingBox to be packed.</param>
    /// <returns>Read-only list of ContainerData reflecting the state of all containers after attempting the packing.</returns>
    private IReadOnlyList<ContainerData> PackBox(PendingBox box) 
    {
        if (! TryPackBox(box)) // trying to pack failed
        {
            // checking if the box overall is not heavier than the capacity itself
            if (box.BoxProperties.Weight > _containerProperties.MaxWeight)
            {
                throw new Exception($"The box is too heavy! Box weight: {box.BoxProperties.Weight} > Max container weight: {_containerProperties.MaxWeight}");
            }

            // before opening a new container, we try to fit with different rotations
            if(! TryPackWithAnyRotation(box))
            {
                AddContainer();

                // if adding fails even after adding an empty container, it means that the rotated sizes are greater than empty container itself
                if (! TryPackBox(box)) 
                {
                    // if packing fails even with any rotation, the box is simply too large to place
                    if (! TryPackWithAnyRotation(box))
                    {
                        throw new Exception("The box is too large to fit in container with any possible rotation!");
                    }
                }
            }
        }
        return ContainersData;
    }

    /// <summary>
    /// Adds a new empty container to the list of opened containers.
    /// </summary>
    private void AddContainer()
    {
        _containers.Add(new OpenedContainer(_containers.Count, _containerProperties));
    }


    private IList<ContainerData>GetContainersData()
    {
        // collecting the data for heuristics from all the opened containers

       return _containers.Select(container => container.Data).ToList();
    }

    /// <summary>
    /// Tries to pack a box in opened containers using its placement heuristic.
    /// </summary>
    private bool TryPackBox(PendingBox box)
    {
        // trying to pack to a container and at a coordinate chosen by the heuristics; in case no valid region is found, false is returned
        Placement? possiblePlacement = box.PlacementHeuristic(box, GetContainersData());
        if (possiblePlacement != null)
        {
            Placement placement = (Placement)possiblePlacement;
            _containers[placement.ContainerID].PackBox(box, placement);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Tries all rotations of a box until it can be packed in one of the containers.
    /// </summary>
    private bool TryPackWithAnyRotation(PendingBox box) 
    {
        int count = Enum.GetValues(typeof(Rotation)).Length;

        PendingBox newBox;
        for (int i = 1; i < count; i++)
        {
            // using first fit, the first rotation that ensures the box can fit in empty container is returned
            Rotation newRotation = (Rotation)((((int)box.Rotation) + i) % count);
            newBox = new PendingBox(box.BoxProperties, newRotation, box.PlacementHeuristic);

            if (TryPackBox(newBox))
            {
                return true;
            }
        }
        return false;
    }

}











