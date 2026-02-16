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


public static class PackedBoxExtensionForExport
{
    public static PackedBoxExport ExportPackedBox(this PackedBox packedBox)
    {
        return new PackedBoxExport(packedBox.BoxProperties.ID, packedBox.Rotation.ToString(), packedBox.PlacementInfo.OccupiedRegion);
    }
}

