namespace EvolutionaryContainerPacking.Packing;

using EvolutionaryContainerPacking.Packing.Rules.Decoding;
using EvolutionaryContainerPacking.Packing.Rules;
using EvolutionaryContainerPacking.Packing.Architecture.Boxes;
using EvolutionaryContainerPacking.Packing.Architecture;
using EvolutionaryContainerPacking.Packing.Architecture.Containers;

/// <summary>
/// Main entry point for solving a bin packing problem using a set of packing rules.
/// Decodes the packing rules into a sequence of pending boxes and uses BoxPacker to assign them to containers.
/// </summary>
public class PackingSolver
{
    /// <summary>
    /// Decoder that transforms a PackingRules vector into a list of PendingBoxes.
    /// </summary>
    private PackingRulesDecoder _packingRulesDecoder { get; init; }

    /// <summary>
    /// Boxes and container properties.
    /// </summary>
    private PackingInput _packingInput { get; init; }

    /// <summary>
    /// Initializes a solver with a pre-built decoder.
    /// </summary>
    /// <param name="packingInput">Input data including boxes and container properties.</param>
    /// <param name="packingRulesDecoder">Decoder that translates packing vectors into boxes.</param>
    public PackingSolver(PackingInput packingInput, PackingRulesDecoder packingRulesDecoder)
    {
        _packingRulesDecoder = packingRulesDecoder;
        _packingInput = packingInput;
    }

    /// <summary>
    /// Initializes a solver by creating a decoder from a packing setting.
    /// </summary>
    /// <param name="packingInput">Input data including boxes and container properties.</param>
    /// <param name="packingSetting">Settings used to configure the decoder.</param>
    public PackingSolver(PackingInput packingInput, PackingSetting packingSetting)
    {
        _packingInput = packingInput;
        _packingRulesDecoder = new PackingRulesDecoder(packingSetting, packingInput);

    }

    /// <summary>
    /// Solves the packing problem for a given packing rules.
    /// <para>
    /// 1. Decodes the packing rules into a list of PendingBoxes using the decoder.
    /// 2. Uses BoxPacker to assign boxes into containers according to their heuristics and container constraints.
    /// 3. Returns the final list of containers with packed boxes.
    /// </para>
    /// </summary>
    /// <param name="packingRules">The packing rules guiding box order, rotation and placement heuristic.</param>

    public IReadOnlyList<ContainerData> Solve(PackingRules packingRules)
    {
        IReadOnlyList<PendingBox> boxesToBePacked = _packingRulesDecoder.Decode(packingRules);
        BoxPacker boxPacker = new BoxPacker(_packingInput.ContainerProperties);
        IReadOnlyList<ContainerData> containers = boxPacker.PackBoxes(boxesToBePacked);

        return containers;
    }

    /// <summary>
    /// Estimates the expected length of the packing rules required for the solver.
    /// Takes into account rotations, selected heuristics, and placement heuristics.
    /// </summary>
    /// <param name="packingInput">Input data.</param>
    /// <param name="packingSetting">Packing settings affecting vector length.</param>
    /// <returns>Estimated minimal length of the packing rules.</returns>
    public static int GetPackingRulesMinimalLength(PackingInput packingInput, PackingSetting packingSetting)
    {
        int i = 0;
        if (packingSetting.AllowRotations)
        {
            i++;
        }
        if (packingSetting.SelectedPackingOrderHeuristic == null)
        {
            i++;
        }
        if (packingSetting.SelectedPlacementHeuristics.Length > 1)
        {
            i++;
        }
        return i * packingInput.BoxPropertiesList.Count;
    }

    /// <summary>
    /// Computes a simple lower bound on the number of containers needed.
    /// Uses the maximum of total weight / container max weight and total volume / container volume.
    /// </summary>
    /// <returns>Estimated minimum number of containers required.</returns>
    public static int GetLowerBound(PackingInput packingInput)
    {
        double weight = packingInput.BoxPropertiesList.Sum(x => x.Weight);
        double volume = packingInput.BoxPropertiesList.Sum(x => x.Sizes.GetVolume());

        return (int)Math.Ceiling(
            Math.Max(
            weight / packingInput.ContainerProperties.MaxWeight,
            volume / packingInput.ContainerProperties.Sizes.GetVolume()
            )
            );
    }
}

