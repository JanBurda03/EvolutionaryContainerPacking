namespace EvolutionaryContainerPacking.Packing.Architecture.EMR;

using EvolutionaryContainerPacking.Packing.Architecture.Geometry;
using System.Collections.Generic;
/// <summary>
/// Maintains a list of maximal empty 3D regions in a container.
/// Provides methods to update regions after placing a new box and 
/// ensures regions remain maximal (no region is completely covered by another).
/// </summary>
public class EmptyMaximalRegions
{
    /// <summary>
    /// Public read-only view of the empty maximal regions.
    /// </summary>
    public IReadOnlyList<Region> EmptyMaximalRegionsList => _emptyMaximalRegions.AsReadOnly();

    private IList<Region> _emptyMaximalRegions;

    // Responsible for merging regions that start at the same Z height
    private readonly EMRMerger _merger;

    // Responsible for splitting regions based on new occupied box
    private readonly EMRSplitter _splitter;

    public EmptyMaximalRegions(Region initial)
    {
        _emptyMaximalRegions = new List<Region> { initial };
        _splitter = new EMRSplitter();
        _merger = new EMRMerger(); // ⚠️ was missing in original code
    }

    public EmptyMaximalRegions(Sizes sizes)
    {
        _emptyMaximalRegions = new List<Region> { sizes.ToRegion() };
        _splitter = new EMRSplitter();
        _merger = new EMRMerger(); // ⚠️ added
    }

    /// <summary>
    /// Updates the list of empty maximal regions after placing a new box.
    /// Splits intersecting regions, removes subregions, and adds new top regions merged appropriately.
    /// </summary>
    /// <param name="newOccupied">Region of the newly placed box.</param>
    public void Update(Region newOccupied)
    {
        // Validate that the placement is inside an existing empty region
        if (!IsValidPlacement(newOccupied))
        {
            throw new Exception("The region is already occupied!");
        }

        List<Region> newRegions = new List<Region>();
        List<Region> unchangedRegions = new List<Region>();

        foreach (Region region in _emptyMaximalRegions)
        {
            // Split intersecting regions and add to newRegions
            if (region.IntersectsWith(newOccupied))
            {
                newRegions.AddRange(_splitter.SplitRegion(region, newOccupied));
            }
            else
            {
                unchangedRegions.Add(region);
            }
        }

        // Remove subregions from the newly created regions
        newRegions = DeleteSubregions(newRegions, unchangedRegions);

        // Combine unchanged regions and new regions
        List<Region> allRegions = new List<Region>();
        allRegions.AddRange(unchangedRegions);
        allRegions.AddRange(newRegions);

        // Add region above the placed box up to the top of container, if applicable
        int boxRegionUpperHeight = newOccupied.End.Z;
        int containerHeight = _emptyMaximalRegions[0].End.Z;

        if (containerHeight != boxRegionUpperHeight)
        {
            var upperRegion = new Region(
                new Coordinates(newOccupied.Start.X, newOccupied.Start.Y, boxRegionUpperHeight),
                new Coordinates(newOccupied.End.X, newOccupied.End.Y, containerHeight)
            );

            // Find all regions that start at the top of the box
            var sameBaseRegions = allRegions
                .Where(reg => reg.Start.Z == boxRegionUpperHeight)
                .ToList();

            // Merge new top region with any existing regions at the same Z
            var newMerged = _merger.Merge(upperRegion, sameBaseRegions);

            allRegions.AddRange(newMerged);
        }

        allRegions = DeleteSubregions(allRegions);

        _emptyMaximalRegions = allRegions;
    }

    /// <summary>
    /// Checks if the new box fits entirely within at least one empty maximal region.
    /// </summary>
    private bool IsValidPlacement(Region newlyOccupied)
    {
        foreach (Region space in _emptyMaximalRegions)
        {
            if (newlyOccupied.IsSubregionOf(space))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Removes any regions that are subregions of other regions (only considers new regions).
    /// </summary>
    private List<Region> DeleteSubregions(IReadOnlyList<Region> newReg)
    {
        var newRegions = newReg.Distinct().ToList();

        List<Region> toRemove = new List<Region>();

        for (int i = 0; i < newRegions.Count; i++)
        {
            for (int j = i + 1; j < newRegions.Count; j++)
            {
                var a = newRegions[i];
                var b = newRegions[j];

                if (a.IsSubregionOf(b))
                    toRemove.Add(a);
                else if (b.IsSubregionOf(a))
                    toRemove.Add(b);
            }
        }

        foreach (Region region in toRemove)
        {
            newRegions.Remove(region);
        }

        return newRegions;
    }

    /// <summary>
    /// Removes subregions from new regions considering existing static regions.
    /// </summary>
    private List<Region> DeleteSubregions(IReadOnlyList<Region> newReg, IReadOnlyList<Region> staticRegions)
    {
        var newRegions = newReg.Distinct().ToList();

        List<Region> toRemove = new List<Region>();

        // Remove new regions that are subregions of other new regions
        for (int i = 0; i < newRegions.Count; i++)
        {
            for (int j = i + 1; j < newRegions.Count; j++)
            {
                var a = newRegions[i];
                var b = newRegions[j];

                if (a.IsSubregionOf(b))
                    toRemove.Add(a);
                else if (b.IsSubregionOf(a))
                    toRemove.Add(b);
            }
        }

        // Remove new regions that are subregions of existing unchanged regions
        foreach (Region region in staticRegions)
        {
            foreach (Region newRegion in newRegions)
            {
                if (newRegion.IsSubregionOf(region))
                {
                    toRemove.Add(newRegion);
                }
            }
        }

        foreach (Region region in toRemove)
        {
            newRegions.Remove(region);
        }

        return newRegions;
    }
}




