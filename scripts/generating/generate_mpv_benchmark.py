"""
MPV benchmark generator.

Generates the full set of 320 benchmark instances defined by
Martello, Pisinger, and Vigo (2000).

Structure:
  - 8 classes
  - 4 instance sizes (n = 50, 100, 150, 200)
  - 10 instances per (class, n)

Instances are generated deterministically using fixed seeds
and stored in a structured directory hierarchy.
"""

from pathlib import Path
from mpv_generator import generate_mpv_instance
from saver import save_to_file


def generate_full_mpv_benchmark(
    output_root: Path = Path("../../data/mpv")
):
    for class_id in range(1, 9):
        for n in [50, 100, 150, 200]:
            for v in range(1, 11):
                seed = n + v  # exact MPV seed

                instance = generate_mpv_instance(
                    class_id=class_id,
                    box_count=n,
                    seed=seed
                )

                target_dir = (
                    output_root
                    / f"class{class_id}"
                    / f"n{n}"
                )

                filename = f"instance_{v:02d}.json"
                save_to_file(instance, target_dir / filename)


if __name__ == "__main__":
    generate_full_mpv_benchmark()
