import argparse
import json
import subprocess
from pathlib import Path
import tempfile
import sys
import re


def main(input_dir, output_dir, solver_exe, packing_setting_str, evolution_setting_str):
    """
    Main batch execution logic.

    Iterates over all JSON files in the input directory, creates a temporary
    configuration file for each of them, and runs the C# solver executable.
    """

    input_dir = Path(input_dir)
    output_dir = Path(output_dir)
    solver_exe = Path(solver_exe)

    # Ensure output directory exists
    output_dir.mkdir(parents=True, exist_ok=True)

    # Parse JSON settings passed as strings
    try:
        packing_setting = json.loads(packing_setting_str)
        evolution_setting = json.loads(evolution_setting_str)
    except json.JSONDecodeError as e:
        print(f"Error parsing JSON settings: {e}")
        sys.exit(1)

    # Find all JSON files in input directory (recursively)
    input_files = sorted(input_dir.rglob("*.json"))

    if not input_files:
        print("No JSON files found in input directory.")
        return

    print(f"Found {len(input_files)} input files.")

    # Create a temporary config file that will be reused
    with tempfile.NamedTemporaryFile(mode="w", suffix=".json", delete=False) as tmp:
        temp_config_path = Path(tmp.name)

    for input_file in input_files:
        # Extract filename without extension
        base_name = input_file.stem

        # Determine relative path of the input file with respect to input_dir
        relative_path = input_file.relative_to(input_dir)

        # Replicate the same directory structure inside output_dir
        output_subdir = output_dir / relative_path.parent
        output_subdir.mkdir(parents=True, exist_ok=True)

        output_file = output_subdir / f"output_{base_name}.json"
        stats_file = output_subdir / f"evolutionStatistics_{base_name}.csv"




        # Get number of packages from folder nXX
        numner_of_boxes = None
        for part in reversed(relative_path.parts):
            match = re.match(r"n(\d+)", part)
            if match:
                numner_of_boxes = int(match.group(1))
                break

        if  numner_of_boxes is None:
            print(f"Could not determine number of packages for {input_file}")
            continue

        # Copy evolution setting so we don't overwrite the original
        evolution_setting_for_run = evolution_setting.copy()

        # Convert IndividualsCoef -> NumberOfIndividuals
        individuals_coef = evolution_setting_for_run.pop("IndividualsCoef")
        evolution_setting_for_run["NumberOfIndividuals"] = individuals_coef *   numner_of_boxes

        # Build configuration JSON
        config = {
            "SourceFile": str(input_file),
            "OutputFile": str(output_file),
            "EvolutionStatisticsFile": str(stats_file),
            "PackingSetting": packing_setting,
            "EvolutionAlgorithmSetting": evolution_setting_for_run,
        }

        # Write configuration to temp file
        with open(temp_config_path, "w") as f:
            json.dump(config, f, indent=4)

        print(f"Running solver for {input_file}...")

        try:
            subprocess.run(
                [str(solver_exe), str(temp_config_path)],
                check=True
            )
        except subprocess.CalledProcessError as e:
            print(f"Solver failed for {input_file.name}: {e}")
            continue

    print("Batch processing finished.")

if __name__ == "__main__":
    parser = argparse.ArgumentParser(
        description="Batch runner for C# packing solver."
    )

    parser.add_argument(
        "--input-dir",
        default="..\\data\\mpv\\original",
        help="Directory containing input JSON files."
    )

    parser.add_argument(
        "--output-dir",
        default="..\\experiments\\results\\original",
        help="Directory where output files will be written."
    )

    parser.add_argument(
        "--solver_exe",
        default="..\\src\\App\\bin\\Release\\net8.0-windows\\App.exe",
        help="Path to the compiled C# solver executable."
    )

    parser.add_argument(
        "--packing-setting",
        default="..\\experiments\\settings\\packing\\max_distance__no_order.json",
        help="Path to JSON file describing PackingSetting."
    )

    parser.add_argument(
        "--evolution-setting",
        default="..\\experiments\\settings\\evolution\\elitist__no_hill_climbing.json",
        help="Path to JSON file describing EvolutionAlgorithmSetting."
    )

    args = parser.parse_args()

    with open(args.packing_setting, "r") as f:
        packing_setting_str = f.read()

    with open(args.evolution_setting, "r") as f:
        evolution_setting_str = f.read()

    main(
        args.input_dir,
        args.output_dir,
        args.solver_exe,
        packing_setting_str,
        evolution_setting_str
    )
