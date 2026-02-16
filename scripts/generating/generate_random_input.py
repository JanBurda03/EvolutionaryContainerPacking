"""
Entry point for random packing input generation.

This script generates a single dataset instance and stores it
as a JSON file for later use by the C# application.
"""

from pathlib import Path
from models import Sizes, ContainerProperties, PackingInput
from random_box_factory import RandomBoxPropertyFactory
from saver import save_to_file


def generate(
    output_path: Path = Path("../data/inputs/packing_input.json"),
    box_count: int = 100,
    min_box_size: int = 5,
    max_box_size: int = 20,
    min_density: float = 0.01,
    max_density: float = 0.05,
    container_sizes: Sizes = Sizes(50, 40, 30),
    max_container_weight: int = 2**31 - 1,
    seed: int = 42
):
    """
    Generates a packing input dataset.

    Args:
        output_path: Target JSON file path.
        box_count: Number of boxes to generate.
        min_box_size: Minimum box edge size.
        max_box_size: Maximum box edge size.
        min_density: Minimum density multiplier.
        max_density: Maximum density multiplier.
        container_sizes: Dimensions of the container.
        max_container_weight: Maximum allowed container weight.
        seed: Random seed for reproducibility.
    """
    factory = RandomBoxPropertyFactory(
        min_size=min_box_size,
        max_size=max_box_size,
        min_density=min_density,
        max_density=max_density,
        seed=seed
    )

    boxes = factory.create_multiple(box_count)

    container = ContainerProperties(
        sizes=container_sizes,
        max_weight=max_container_weight
    )

    packing_input = PackingInput(container, boxes)
    save_to_file(packing_input, output_path)


if __name__ == "__main__":
    import argparse

    parser = argparse.ArgumentParser(
        description="Random input generator for bin packing experiments"
    )

    parser.add_argument("--output", type=Path, default=Path("../data/inputs/packing_input.json"))
    parser.add_argument("--boxes", type=int, default=100)
    parser.add_argument("--min-size", type=int, default=5)
    parser.add_argument("--max-size", type=int, default=20)
    parser.add_argument("--min-density", type=float, default=0.01)
    parser.add_argument("--max-density", type=float, default=0.05)
    parser.add_argument("--container-x", type=int, default=50)
    parser.add_argument("--container-y", type=int, default=40)
    parser.add_argument("--container-z", type=int, default=30)
    parser.add_argument("--seed", type=int, default=42)

    args = parser.parse_args()

    generate(
        output_path=args.output,
        box_count=args.boxes,
        min_box_size=args.min_size,
        max_box_size=args.max_size,
        min_density=args.min_density,
        max_density=args.max_density,
        container_sizes=Sizes(
            args.container_x,
            args.container_y,
            args.container_z
        ),
        seed=args.seed
    )
