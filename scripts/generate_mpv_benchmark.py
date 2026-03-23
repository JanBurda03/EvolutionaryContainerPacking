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
  - Container density parameter controlling the maximum allowed total weight.
  - Automatic generation of three benchmark categories:
        original  -> no weight constraints
        medium    -> mean box density = 1.2
        heavy     -> mean box density = 1.5
"""


def generate_full_mpv_benchmark(output_root: Path):
    """
    Generates the full MPV benchmark with 3 variants:

    original/
        -> no weight (mean_box_density = 0)

    medium/
        -> mean_box_density = 1.2

    heavy/
        -> mean_box_density = 1.5
    """

    # ---- ORIGINAL (no weights) ----
    generate_mpv_benchmark(
        output_root=output_root / "original",
        mean_box_density=0,
        std_box_density=0
    )

    # ---- MEDIUM ----
    medium_mean = 1.2
    medium_std = 0.1 * medium_mean

    generate_mpv_benchmark(
        output_root=output_root / "medium",
        mean_box_density=medium_mean,
        std_box_density=medium_std
    )

    # ---- HEAVY ----
    heavy_mean = 1.5
    heavy_std = 0.1 * heavy_mean

    generate_mpv_benchmark(
        output_root=output_root / "heavy",
        mean_box_density=heavy_mean,
        std_box_density=heavy_std
    )


def generate_mpv_benchmark(output_root: Path, mean_box_density=0, std_box_density=0):
    """
    Generates the full MPV benchmark:
      - Classes 1–8
      - 4 instance sizes: 50, 100, 150
      - 10 instances per (class, size)
    """

    instance_sizes = [50, 100, 150]
    instance_count = 10

    for class_id in range(1, 9):
        for n in instance_sizes:
            for v in range(1, instance_count + 1):
                seed = n + v
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
    



if __name__ == "__main__":
    parser = argparse.ArgumentParser(
        description="Full MPV benchmark generator (original + weighted classes)"
    )
    parser.add_argument(
        "--output",
        type=Path,
        default=Path("../data/mpv"),
        help="Root directory for generated benchmark instances"
    )

    args = parser.parse_args()
    generate_full_mpv_benchmark(args.output)