namespace EvolutionaryContainerPacking.App.Output;

using EvolutionaryContainerPacking.Packing.Architecture.Containers;

/// <summary>
/// Provides export extension methods for <see cref="ContainerData"/>.
/// </summary>
public static class ContainerDataExtensionForExport
{
    /// <summary>
    /// Converts <see cref="ContainerData"/> into its export representation.
    /// </summary>
    /// <param name="container">Container to export.</param>
    /// <returns>Export-ready container object.</returns>
    public static ContainerExport ExportContainer(this ContainerData container)
    {
        return new ContainerExport(
            container.ID,
            container.Weight,
            container.OccupiedVolume,
            (from box in container.PlacedBoxes select box.ExportPackedBox()).ToList());
    }
}