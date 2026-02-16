"""
Random generation of box properties.

This module is responsible for generating random box dimensions
and weights based on configurable size and density ranges.
"""

import random
from typing import List
from models import Sizes, BoxProperties


class RandomBoxPropertyFactory:
    """
    Factory class for creating random BoxProperties instances.
    """

    def __init__(
        self,
        min_size: int,
        max_size: int,
        min_density: float,
        max_density: float,
        seed: int = 42
    ):
        """
        Initializes the random generator.

        Args:
            min_size: Minimum edge length of a box.
            max_size: Maximum edge length of a box.
            min_density: Lower bound of density multiplier.
            max_density: Upper bound of density multiplier.
            seed: Optional random seed for reproducibility.
        """
        self.min_size = min_size
        self.max_size = max_size
        self.min_density = min_density
        self.max_density = max_density
        self.random = random.Random(seed)
        self.current_id = 0

    def _next_size(self) -> int:
        """
        Generates a random edge size.
        """
        return self.random.randint(self.min_size, self.max_size)

    def _next_density(self) -> float:
        """
        Generates a random density value.
        """
        return self.random.uniform(self.min_density, self.max_density)

    def create(self) -> BoxProperties:
        """
        Creates a single randomly parameterized box.
        """
        sizes = Sizes(
            self._next_size(),
            self._next_size(),
            self._next_size()
        )

        weight = int(sizes.volume() * self._next_density())

        box = BoxProperties(
            id=self.current_id,
            sizes=sizes,
            weight=weight
        )

        self.current_id += 1
        return box

    def create_multiple(self, count: int) -> List[BoxProperties]:
        """
        Creates multiple boxes in sequence.
        """
        return [self.create() for _ in range(count)]
