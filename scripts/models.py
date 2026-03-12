"""
Data models used for packing input generation.

These classes represent a minimal, serializable data structure
shared between Python (data generation) and C# (algorithm runtime).
"""

from dataclasses import dataclass, asdict
from typing import List


@dataclass
class Sizes:
    """
    Represents 3D dimensions of an object.
    """
    x: int
    y: int
    z: int

    def volume(self) -> int:
        """
        Computes volume of the object.
        """
        return self.x * self.y * self.z


@dataclass
class BoxProperties:
    """
    Represents a single box to be packed.
    """
    id: int
    sizes: Sizes
    weight: int


@dataclass
class ContainerProperties:
    """
    Represents the container into which boxes are packed.
    """
    sizes: Sizes
    max_weight: int


@dataclass
class PackingInput:
    """
    Root input structure consumed by the bin packing algorithm.
    """
    container: ContainerProperties
    boxes: List[BoxProperties]

    def to_dict(self) -> dict:
        """
        Converts the object into a dictionary suitable for JSON serialization.
        """
        return {
            "ContainerProperties": {
                "Sizes": {
                    "X": self.container.sizes.x,
                    "Y": self.container.sizes.y,
                    "Z": self.container.sizes.z
                },
                "MaxWeight": self.container.max_weight
            },
            "BoxPropertiesList": [
                {
                    "ID": box.id,
                    "Sizes": {
                        "X": box.sizes.x,
                        "Y": box.sizes.y,
                        "Z": box.sizes.z
                    },
                    "Weight": box.weight
                }
                for box in self.boxes
            ]
        }

