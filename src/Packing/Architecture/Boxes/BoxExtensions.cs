namespace EvolutionaryContainerPacking.Packing.Architecture.Boxes;

using EvolutionaryContainerPacking.Packing.Architecture.Geometry;

/// <summary>
/// Extension methods shared by all box types implementing IBox.
/// </summary>
public static class BoxExtensions
{
    /// <summary>
    /// Gets the original unrotated sizes of the box.
    /// </summary>
    public static Sizes GetOriginalSizes(this IBox box) => box.BoxProperties.Sizes;

    /// <summary>
    /// Gets the sizes of the box after rotation.
    /// </summary>
    public static Sizes GetRotatedSizes(this IBox box) => box.BoxProperties.Sizes.GetRotatedSizes(box.Rotation);

    /// <summary>
    /// Gets the volume of the box.
    /// </summary>
    public static long GetVolume(this IBox box) => box.BoxProperties.Sizes.GetVolume();
}
