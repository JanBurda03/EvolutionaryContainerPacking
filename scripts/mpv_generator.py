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

# --- Configuration of box dimension ranges per type ---
BOX_TYPE_RANGES = {
    1: ((0, 1/2), (2/3, 1), (2/3, 1)),
    2: ((2/3, 1), (2/3, 1), (0, 1/2)),
    3: ((2/3, 1), (0, 1/2), (2/3, 1)),
    4: ((1/2, 1), (1/2, 1), (1/2, 1)),
    5: ((0, 1/2), (0, 1/2), (0, 1/2)),
    6: ((0, 1), (0, 1), (0, 1)),
    7: ((0, 7/8), (0, 7/8), (0, 7/8)),
    8: ((0, 1), (0, 1), (0, 1))
}

def generate_box_dimensions(rng: random.Random, X: int, Y: int, Z: int, box_type: int) -> tuple[int,int,int]:
    """
    Generates integer dimensions for a box of a given type based on BOX_TYPE_RANGES.

    Args:
        rng: Random number generator
        X, Y, Z: Container dimensions
        box_type: Type 1–8

    Returns:
        Tuple (X_box, Y_box, Z_box)
    """
    if box_type not in BOX_TYPE_RANGES:
        raise ValueError(f"Invalid box type {box_type}, must be 1–8")

    x_frac_range, y_frac_range, z_frac_range = BOX_TYPE_RANGES[box_type]

    X_box = max(1, int(rng.uniform(*x_frac_range) * X))
    Y_box = max(1, int(rng.uniform(*y_frac_range) * Y))
    Z_box = max(1, int(rng.uniform(*z_frac_range) * Z)) 

    return X_box, Y_box, Z_box

def get_container_size_for_class(class_id: int) -> int:
    """
    Returns the container edge length for a given MPV class.
    """
    sizes = {1: 100, 2: 100, 3: 100, 4: 100, 5: 100, 6: 10, 7: 40, 8: 100}
    if class_id not in sizes:
        raise ValueError("Class must be 1–8")
    return sizes[class_id]

def choose_box_type_for_class(rng: random.Random, class_id: int) -> int:
    """
    Chooses a box type for classes 1–5 based on MPV benchmark probabilities:
    - 60% chance for the box type equal to class_id
    - 10% chance for other types
    """
    if class_id > 5:
        return class_id  # types 6–8 are deterministic

    types = [1, 2, 3, 4, 5]
    weights = [0.1] * 5
    weights[class_id - 1] = 0.6
    return rng.choices(types, weights)[0]

def generate_mpv_instance(
    class_id: int,
    box_count: int,
    seed: int = 42,
    container_density: float = 0.05,
    mean_box_density: float = 0.03,
    std_box_density: float = 0.005
) -> PackingInput:
    """
    Generates a single MPV benchmark instance with box weights.

    Args:
        class_id: MPV class (1–8)
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
    SIZE = get_container_size_for_class(class_id)
    X = Y = Z = SIZE
    container_volume = X * Y * Z
    max_weight = int(container_volume * container_density)
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

        boxes.append(
            BoxProperties(
                id=i,
                sizes=Sizes(X_box, Y_box, Z_box),
                weight=weight
            )
        )

    return PackingInput(container, boxes)