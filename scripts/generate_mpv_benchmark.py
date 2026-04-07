import argparse
from pathlib import Path
from mpv_generator import generate_mpv_instance
from saver import save_to_file

"""
MPV Benchmark Generator (Python version)

This Python code generates 3D bin packing instances inspired by
the benchmark instances of Martello, Pisinger, and Vigo (2000, 2003).

Original papers describing the instances:

  S. Martello, D. Pisinger, D. Vigo, E. den Boef, J. Korst (2003)
  "Algorithms for General and Robot-packable Variants of the
   Three-Dimensional Bin Packing Problem", submitted TOMS.

  S. Martello, D. Pisinger, D. Vigo (2000)
  "The three-dimensional bin packing problem",
  Operations Research, 48, 256-267

Copyright notice (original 3DBPP algorithm and instance descriptions):
  (c) 1998, 2003, 2006
  David Pisinger, Silvano Martello, Daniele Vigo
  DIKU, University of Copenhagen / DEIS, University of Bologna

Official 3DBPP page: https://hjemmesider.diku.dk/~pisinger/new3dbpp/readme.3dbpp

This Python implementation is independent code, written for
research and academic purposes, and uses the above publications
as reference for instance design. 

Additional features in this Python version:
  - Support for box weights and container maximum weight
    (weight constraints).
  - Randomized box densities with configurable mean and standard deviation.
"""


def generate_mpv_benchmark(output_root: Path, mean_box_density=0, std_box_density=0, instance_sizes=[50, 100, 150], instance_count=10, initial_seed=0):
    """
    Generates a configurable MPV benchmark dataset.

    The generator creates instances for:
      - Classes 1–4
      - Instance sizes defined by `instance_sizes`
      - `instance_count` instances for each (class, size) combination

    Seed handling:
      - For each instance, the seed is computed as:
        seed = n + v + initial_seed
      - where:
        n = instance size
        v = instance index (starting from 1)

    Parameters:
        output_root (Path):
            Root directory where generated instances will be saved.

        mean_box_density (float):
            Mean box density used for weighted instances.

        std_box_density (float):
            Standard deviation of box density.

        instance_sizes (list[int]):
            List of instance sizes to generate.

        instance_count (int):
            Number of instances per size.

        initial_seed (int):
            Seed offset added to each generated instance.
    """

    NUMBER_OF_CLASSES = 4

    for class_id in range(1, NUMBER_OF_CLASSES+1):
        for n in instance_sizes:
            for v in range(1, instance_count + 1):
                seed = n + v + initial_seed
                instance = generate_mpv_instance(
                    class_id=class_id,
                    box_count=n,
                    seed=seed,
                    container_density=1,
                    mean_box_density=mean_box_density,
                    std_box_density=std_box_density
                )
                target_dir = output_root / f"class{class_id}" / f"n{n}"
                filename = f"instance_{v:02d}.json"
                save_to_file(instance, target_dir / filename)
    


def parse_instance_sizes(value: str) -> list[int]:
    return [int(x.strip()) for x in value.split(",") if x.strip()]


if __name__ == "__main__":
    parser = argparse.ArgumentParser(
        description="MPV benchmark generator"
    )
    parser.add_argument(
        "--output",
        type=Path,
        default=Path("../data/mpv"),
        help="Root directory where generated instances will be saved",
    )
    parser.add_argument(
        "--mean-box-density",
        type=float,
        default=0.0,
        help="Mean box density",
    )
    parser.add_argument(
        "--std-box-density",
        type=float,
        default=0.0,
        help="Standard deviation of box density",
    )
    parser.add_argument(
        "--instance-sizes",
        type=parse_instance_sizes,
        default=[50, 100, 150],
        help="Comma-separated list of instance sizes, e.g. 50,100,150",
    )
    parser.add_argument(
        "--instance-count",
        type=int,
        default=10,
        help="Number of instances per size",
    )
    parser.add_argument(
        "--initial-seed",
        type=int,
        default=0,
        help="Seed offset added to each generated instance",
    )

    args = parser.parse_args()

    generate_mpv_benchmark(
        output_root=args.output,
        mean_box_density=args.mean_box_density,
        std_box_density=args.std_box_density,
        instance_sizes=args.instance_sizes,
        instance_count=args.instance_count,
        initial_seed=args.initial_seed,
    )