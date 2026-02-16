using EvolutionaryContainerPacking.Packing.Rules.Decoding.PartDecoders;
using EvolutionaryContainerPacking.Packing.Rules.Decoding.Sorting;
using EvolutionaryContainerPacking.Packing.Architecture.Boxes;
using EvolutionaryContainerPacking.Packing.Architecture.Placements;
using EvolutionaryContainerPacking.Packing.Architecture.Geometry;

namespace EvolutionaryContainerPacking.Packing.Rules.Decoding;

/// <summary>
/// Decodes a packing rules vector into a list of boxes to be packed.
/// 
/// The packing rules vector is split into logical parts (sorting order,
/// rotations, placement heuristics) according to
/// the active decoders. These parts are then decoded and combined to
/// produce a sorted list of <see cref="PendingBox"/> instances
/// ready for the packing algorithm.
/// 
/// Any of the three decoders can either be an active decoder,
/// which uses the information from the packing rules, or a
/// static one, which operates without the packing rules
/// (for example heuristical sorting by box volume), 
/// which can be consequently reduced by the part representing 
/// that particular coding
/// </summary>
public class PackingRulesDecoder
{
    private readonly IPackingRulesPartDecoder<PlacementHeuristic> _heuristicDecoder;
    private readonly IPackingRulesPartDecoder<Rotation> _rotationDecoder;
    private readonly IPendingBoxSorter _boxSorter;
    private readonly PackingInput _packingInput;

    private readonly int _numberOfBoxes; // number of boxes for packing, which is also the length of each part of the packing rules (sorting, rotations or placement heuristics), if active

    /// <summary>
    /// Gets the minimal required length of the packing rules. 
    /// Computed as number of active packing rules parts * number of boxes.
    /// </summary>
    public int PackingRulesMinimalLength { get; init; }

    /// <summary>
    /// Creates a new packing rules decoder using explicit decoders and input.
    /// </summary>
    /// <param name="heuristicDecoder">Decoder for placement heuristics part of the packing vector.</param>
    /// <param name="rotationDecoder">Decoder for rotations part of the packing vector.</param>
    /// <param name="boxSorter">Sorter to determine the packing order of boxes.</param>
    /// <param name="packingInput">Input data describing boxes and containers.</param>
    public PackingRulesDecoder(
        IPackingRulesPartDecoder<PlacementHeuristic> heuristicDecoder,
        IPackingRulesPartDecoder<Rotation> rotationDecoder,
        IPendingBoxSorter boxSorter,
        PackingInput packingInput)
    {
        _heuristicDecoder = heuristicDecoder;
        _rotationDecoder = rotationDecoder;
        _boxSorter = boxSorter;
        _packingInput = packingInput;
        _numberOfBoxes = _packingInput.BoxPropertiesList.Count;

        PackingRulesMinimalLength = GetNumberOfPackingRulesParts() * _numberOfBoxes;
    }



    /// <summary>
    /// Creates a new packing rules decoder based on a <see cref="PackingSetting"/> configuration.
    /// The decoders and sorter are automatically selected according to the settings.
    /// </summary>
    /// <param name="packingSetting">The packing settings determining decoders and sorting strategy.</param>
    /// <param name="packingInput">Input data describing boxes and containers.</param>
    /// <returns>A new <see cref="PackingRulesDecoder"/> instance configured according to the settings.</returns>
    public PackingRulesDecoder(PackingSetting packingSetting,
        PackingInput packingInput)
    {
        if (packingSetting.SelectedPlacementHeuristics.Length == 1)
        {
            // if only one heuristic is chosen, there is no need for active packing rules decoding
            _heuristicDecoder =
                new OnePlacementDecoder(
                    PlacementHeuristics.GetPlacementHeuristic(
                        packingSetting.SelectedPlacementHeuristics[0]));
        }
        else
        {
            _heuristicDecoder =
                new MultiplePlacementsDecoder(
                    PlacementHeuristics.GetPlacementHeuristics(
                        packingSetting.SelectedPlacementHeuristics));
        }

        if (packingSetting.AllowRotations)
        {
            _rotationDecoder = new AllRotationsDecoder();
        }
        else
        {
            _rotationDecoder = new OneRotationDecoder(Rotation.XYZ);
        }

        if (packingSetting.SelectedPackingOrderHeuristic == null)
        {
            _boxSorter = new PackingRulesUsingBoxSorter();
        }
        else
        {
            _boxSorter =
                new HeuristicalBoxSorter(
                    OrderHeuristics.GetOrderHeuristic(
                        packingSetting.SelectedPackingOrderHeuristic));
        }


        _packingInput = packingInput;
        _numberOfBoxes = _packingInput.BoxPropertiesList.Count;

        PackingRulesMinimalLength = GetNumberOfPackingRulesParts() * _numberOfBoxes;
    }

    /// <summary>
    /// Decodes the packing rules vector into a sorted list of boxes to be packed.
    /// </summary>
    /// <param name="packingRules">Packing rules vector.</param>
    /// <returns>Sorted list of boxes to be packed.</returns>
    public IReadOnlyList<PendingBox> Decode(PackingRules packingRules)
    {
        PendingBox[] pendingBoxesUnsorted = ConvertToBoxToBePackedList(packingRules);

        PackingRules sortingPart = GetBoxSortingPart(packingRules);

        return _boxSorter.Sort(
            pendingBoxesUnsorted,
            sortingPart);
    }




    private PackingRules GetBoxSortingPart(PackingRules packingRules)
    {
        // First part of the packing vector may be used to determine the order in which boxes are packed
        if (_boxSorter.IsUsingPackingRules)
        {
            return packingRules.Slice(0, _numberOfBoxes);
        }

        // If sorting does not depend on the packing vector, return an empty part
        return PackingRules.CreateEmpty();
    }

    private PackingRules GetRotationsPart(PackingRules packingRules)
    {
        // Rotation part starts after the sorting part (if present)
        if (_rotationDecoder.IsUsingPackingRules)
        {
            int i = 0;

            if (_boxSorter.IsUsingPackingRules)
            {
                i++;
            }

            return packingRules.Slice(
                i * _numberOfBoxes,
                _numberOfBoxes);
        }

        // If rotations are fixed, return a zero-filled part
        return PackingRules.CreateZeros(_numberOfBoxes);
    }

    private PackingRules GetPlacementHeuristicsPart(PackingRules packingRules)
    {
        // Placement heuristics part comes after sorting and rotation parts depending on which of them use the packing vector
        if (_heuristicDecoder.IsUsingPackingRules)
        {
            int i = 0;

            if (_boxSorter.IsUsingPackingRules)
            {
                i++;
            }
            if (_rotationDecoder.IsUsingPackingRules)
            {
                i++;
            }

            return packingRules.Slice(
                i * _numberOfBoxes,
                _numberOfBoxes);
        }

        // If a single heuristic is used, return zeros
        return PackingRules.CreateZeros(_numberOfBoxes);
    }

    private PendingBox[] ConvertToBoxToBePackedList(PackingRules packingRules)
    {
        // Extract relevant parts of the packing vector
        PackingRules rotationsPart = GetRotationsPart(packingRules);
        PackingRules placementHeuristicsPart = GetPlacementHeuristicsPart(packingRules);

        // Decode rotations and placement heuristics for each box
        IReadOnlyList<Rotation> rotations = _rotationDecoder.Decode(rotationsPart);
        IReadOnlyList<PlacementHeuristic> heuristics = _heuristicDecoder.Decode(placementHeuristicsPart);

        // Use decoded data to create boxes ready for packing
        PendingBox[] boxesToBePacked = new PendingBox[_numberOfBoxes];

        for (int i = 0; i < boxesToBePacked.Length; i++)
        {
            boxesToBePacked[i] = new PendingBox(
                _packingInput.BoxPropertiesList[i],
                rotations[i],
                heuristics[i]);
        }

        return boxesToBePacked;
    }

    private int GetNumberOfPackingRulesParts()
    {
        // Count how many logical parts the packing rules have based on which components actually use it
        int i = 0;

        if (_heuristicDecoder.IsUsingPackingRules)
        {
            i++;
        }
        if (_rotationDecoder.IsUsingPackingRules)
        {
            i++;
        }
        if (_boxSorter.IsUsingPackingRules)
        {
            i++;
        }

        return i;
    }

}
