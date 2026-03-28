import random
from typing import List
from models import Sizes, BoxProperties, ContainerProperties, PackingInput

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
"""

CONTAINER_SIZE = 100

# --- Configuration of box dimension ranges per type ---
BOX_TYPE_RANGES = {
    1: ((0, 1/2), (1/2, 1), (1/2, 1)),
    2: ((0, 1/2), (0, 1/2), (1/2, 1)),
    3: ((0, 1/2), (0, 1/2), (0, 1/2)),
    4: ((0, 1), (0, 1), (0, 1)),
}

def generate_box_dimensions(rng: random.Random, X: int, Y: int, Z: int, box_type: int) -> tuple[int,int,int]:
    """
    Generates integer dimensions for a box of a given type based on BOX_TYPE_RANGES.

    Args:
        rng: Random number generator
        X, Y, Z: Container dimensions
        box_type: Type 1–5

    Returns:
        Tuple (X_box, Y_box, Z_box)
    """
    if box_type not in BOX_TYPE_RANGES:
        raise ValueError(f"Invalid box type {box_type}, must be 1–4")

    x_frac_range, y_frac_range, z_frac_range = BOX_TYPE_RANGES[box_type]

    x_min = max(1, int(x_frac_range[0] * X))
    x_max = max(1, int(x_frac_range[1] * X))

    y_min = max(1, int(y_frac_range[0] * Y))
    y_max = max(1, int(y_frac_range[1] * Y))

    z_min = max(1, int(z_frac_range[0] * Z))
    z_max = max(1, int(z_frac_range[1] * Z))

    X_box = rng.randint(x_min, x_max)
    Y_box = rng.randint(y_min, y_max)
    Z_box = rng.randint(z_min, z_max)

    return X_box, Y_box, Z_box


def choose_box_type_for_class(rng: random.Random, class_id: int) -> int:
    """
    Chooses box type based on extended MPV logic:

    class 1–3 → structured merge-friendly boxes
    class 4   → very small boxes
    class 5   → fully random boxes
    """

    if class_id in (1, 2, 3):
        # 60% dominant structured type, 20% other structured types
        structured = [1, 2, 3]
        weights = [0.2, 0.2, 0.2]
        weights[class_id - 1] = 0.6
        return rng.choices(structured, weights=weights)[0]

    elif class_id == 4:
        return 4  # completely random items

    else:
        raise ValueError("class_id must be 1–5")

def generate_mpv_instance(
    class_id: int,
    box_count: int,
    seed: int = 42,
    container_density: float = 1.0,
    mean_box_density: float = 0.0,
    std_box_density: float = 0.0
) -> PackingInput:
    """
    Generates a single MPV benchmark instance with box weights.

    Args:
        class_id: MPV class (1–5)
        box_count: Number of boxes
        seed: Random seed
        container_density: Density factor for container max weight
        mean_box_density: Mean density for boxes
        std_box_density: Standard deviation of box densities

    Returns:
        PackingInput object
    """
    rng = random.Random(seed)

    # Container size and max weight
    X = Y = Z = CONTAINER_SIZE
    container_volume = X * Y * Z
    max_weight = max(1, int(container_volume * container_density))
    container = ContainerProperties(
        sizes=Sizes(X, Y, Z),
        max_weight=max_weight
    )

    boxes: List[BoxProperties] = []

    for i in range(box_count):
        box_type = choose_box_type_for_class(rng, class_id)
        X_box, Y_box, Z_box = generate_box_dimensions(rng, X, Y, Z, box_type)
        volume = X_box * Y_box * Z_box

        # Generate box weight based on normal distribution around mean_box_density
        density = max(0, rng.normalvariate(mean_box_density, std_box_density))
        weight = max(1, int(volume * density))

        # Ensuring that the box always fits at least in an empty container
        weight = min(weight, max_weight)

        boxes.append(
            BoxProperties(
                id=i,
                sizes=Sizes(X_box, Y_box, Z_box),
                weight=weight
            )
        )

    return PackingInput(container, boxes)