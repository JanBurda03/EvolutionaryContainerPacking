"""
JSON serialization utilities.
"""

import json
from pathlib import Path
from models import PackingInput


def save_to_file(packing_input: PackingInput, path: Path):
    """
    Saves PackingInput structure to a JSON file.

    Args:
        packing_input: Data structure to serialize.
        path: Output file path.
    """
    path.parent.mkdir(parents=True, exist_ok=True)

    with open(path, "w", encoding="utf-8") as file:
        json.dump(packing_input.to_dict(), file, indent=2)
