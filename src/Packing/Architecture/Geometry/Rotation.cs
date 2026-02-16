namespace EvolutionaryContainerPacking.Packing.Architecture.Geometry;

/// <summary>
/// Represents all possible orthogonal axis-aligned rotations of a 3D box.
/// Each value defines a permutation of the original (X, Y, Z).
/// </summary>
public enum Rotation
{
    XYZ,
    XZY,
    YXZ,
    YZX,
    ZXY,
    ZYX
}