namespace EvolutionaryContainerPacking.App.Output;

using EvolutionaryContainerPacking.Packing.Architecture.Geometry;
using EvolutionaryContainerPacking.Packing.Architecture.Containers;
using EvolutionaryContainerPacking.Packing.Architecture.Boxes;

public record class ContainerExport(
    int ContainerID,
    long CurrentWeight,
    long OccupiedVolume,
    IReadOnlyList<PackedBoxExport> PackedBoxes
);

public record class PackedBoxExport(
    int BoxID,
    string Rotation,
    Region OccupiedRegion
);


public static class ContainerExtensionForExport
{
    public static ContainerExport ExportContainer(this ContainerData container) 
    {
        return new ContainerExport(
            container.ID, 
            container.Weight, 
            container.OccupiedVolume, 
            (from box in container.PlacedBoxes select box.ExportPackedBox()).ToList());
    }
}


public static class PlacedBoxExtensionForExport
{
    public static PackedBoxExport ExportPackedBox(this PlacedBox box)
    {
        return new PackedBoxExport(box.BoxProperties.ID, box.Rotation.ToString(), box.Placement.OccupiedRegion);
    }
}

