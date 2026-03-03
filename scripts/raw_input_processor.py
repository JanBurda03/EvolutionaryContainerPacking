import os
import json
import argparse
import numpy as np
from models import Sizes, BoxProperties, ContainerProperties, PackingInput

def parse_file(file_path: str, density: float, std: float, container_density: float) -> list[PackingInput]:
    """
    Parse a file with packing problem instances.
    Each problem is converted to its own PackingInput instance.
    """
    packing_inputs = []
    with open(file_path, "r") as f:
        lines = [line.strip() for line in f if line.strip()] 

    if len(lines) == 0:
        return packing_inputs

    # First line: number of problems
    P = int(lines[0])

    i = 1  # start after the first line
    while i < len(lines):
        # Problem number + seed
        header = lines[i].split()
        if len(header) != 2:
            raise ValueError(f"Unexpected header at line {i+1} in {file_path}: {lines[i]}")
        i += 1

        # Container dimensions
        container_dims = list(map(int, lines[i].split()))
        container_size = Sizes(x=container_dims[0],
                               y=container_dims[1],
                               z=container_dims[2])
        max_weight = int(sum(container_dims) * container_density)+1
        container = ContainerProperties(sizes=container_size, max_weight=max_weight)
        i += 1

        # Number of box types
        n_boxes = int(lines[i])
        i += 1

        boxes = []
        box_id = 1
        for _ in range(n_boxes):
            parts = list(map(int, lines[i].split()))
            length, width, height = parts[1], parts[3], parts[5]
            count = parts[7]

            for _ in range(count):
                volume = length * width * height
                sampled_density = np.random.normal(density, std)
                weight = max(0, int(volume * sampled_density))
                boxes.append(BoxProperties(
                    id=box_id,
                    sizes=Sizes(x=length, y=width, z=height),
                    weight=weight
                ))
                box_id += 1
            i += 1

        packing_inputs.append(PackingInput(container=container, boxes=boxes))

    if len(packing_inputs) != P:
        print(f"Warning: expected {P} problems but parsed {len(packing_inputs)} in {file_path}")

    return packing_inputs

def process_folder(input_folder: str, output_folder: str, density: float, std: float, container_density: float):
    """
    Process all raw packing files in input folder.
    Each problem instance gets its own JSON in a subfolder named after the input file.
    """
    if not os.path.exists(output_folder):
        os.makedirs(output_folder)

    for filename in os.listdir(input_folder):
        if filename.lower().startswith("thpack") and filename.lower().endswith(".txt"):
            file_path = os.path.join(input_folder, filename)
            packing_inputs = parse_file(file_path, density, std, container_density)

            # Create subfolder
            base_name = os.path.splitext(filename)[0]
            subfolder = os.path.join(output_folder, base_name)
            if not os.path.exists(subfolder):
                os.makedirs(subfolder)

            # Save each instance
            for idx, packing_input in enumerate(packing_inputs, start=1):
                output_path = os.path.join(subfolder, f"{idx}.json")
                with open(output_path, "w") as f:
                    json.dump(packing_input.to_dict(), f, indent=2)

            print(f"Processed {filename}: {len(packing_inputs)} instances saved in {subfolder}")


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Convert OR-Library thpack files to JSON PackingInput instances.")
    parser.add_argument("--input-folder", type=str, default="../data/raw_inputs/", help="Folder with thpack files")
    parser.add_argument("--output-folder", type=str, default="../data/processed_inputs/", help="Folder for JSON outputs")
    parser.add_argument("--density", type=float, default=0.0, help="Average box density (weight per unit volume)")
    parser.add_argument("--std", type=float, default=0.0, help="Standard deviation for density noise")
    parser.add_argument("--container-density", type=float, default=0.0, help="Density multiplier for container max weight")

    args = parser.parse_args()

    process_folder(args.input_folder, args.output_folder, args.density, args.std, args.container_density)
    print("All thpack files processed successfully!")