namespace EvolutionaryContainerPacking.App.Output;

using EvolutionaryContainerPacking.Packing.Architecture.Boxes;

/// <summary>
/// Provides export extension methods for <see cref="PlacedBox"/>.
/// </summary>
public static class PlacedBoxExtensionForExport
{
    /// <summary>
    /// Converts a <see cref="PlacedBox"/> into its export representation.
    /// </summary>
    /// <param name="box">Placed box to export.</param>
    /// <returns>Export-ready placed box object.</returns>
    public static PlacedBoxExport ExportPackedBox(this PlacedBox box)
    {
        return new PlacedBoxExport(
            box.BoxProperties.ID,
            box.Rotation.ToString(),
            box.Placement.OccupiedRegion);
    }
}

