import argparse
import json
import subprocess
from pathlib import Path
import tempfile
import sys
import csv
from statistics import mean
from collections import defaultdict

# -----------------------------
# Batch runner main logic
# -----------------------------
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

        # Build configuration JSON
        config = {
            "SourceFile": str(input_file),
            "OutputFile": str(output_file),
            "EvolutionStatisticsFile": str(stats_file),
            "PackingSetting": packing_setting,
            "EvolutionAlgorithmSetting": evolution_setting,
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

    # -----------------------------
    # Aggregation per nXXX folder
    # -----------------------------
    print("Aggregating results per nXXX folder...")

    for class_dir in output_dir.glob("class*"):
        if not class_dir.is_dir():
            continue
        for n_dir in class_dir.glob("n*"):
            aggregate_generation_csv(n_dir)

    # -----------------------------
    # Final summary CSV
    # -----------------------------
    final_summary_csv = output_dir / "summary.csv"
    final_summary(output_dir, final_summary_csv)


# -----------------------------
# Aggregation functions
# -----------------------------


def aggregate_generation_csv(n_dir: Path):
    """
    Aggregate all evolution CSV files inside a nXXX folder
    and compute average per generation.
    """
    csv_files = list(n_dir.glob("evolutionStatistics_*.csv"))
    if not csv_files:
        print(f"No CSV files found in {n_dir}")
        return

    gen_data = defaultdict(lambda: {"best": [], "average": [], "elapsed": []})

    # Read all CSVs
    for csv_path in csv_files:
        with open(csv_path, newline="", encoding="utf-8-sig") as f:
            reader = csv.DictReader(f, delimiter=";")
            for row in reader:
                gen_num = int(row["Generation"])
                gen_data[gen_num]["best"].append(float(row["Best"].replace(",", ".")))
                gen_data[gen_num]["average"].append(float(row["Average"].replace(",", ".")))
                gen_data[gen_num]["elapsed"].append(float(row["ElapsedSeconds"].replace(",", ".")))

    # Write aggregated CSV
    output_file = n_dir / "averages.csv"
    with open(output_file, "w", newline="", encoding="utf-8") as f:
        fieldnames = ["Generation", "AverageBest", "AverageAverage", "AverageTime"]
        writer = csv.DictWriter(f, fieldnames=fieldnames, delimiter=";")
        writer.writeheader()
        for gen_num in sorted(gen_data.keys()):
            data = gen_data[gen_num]
            writer.writerow({
                "Generation": gen_num,
                "AverageBest": mean(data["best"]),
                "AverageAverage": mean(data["average"]),
                "AverageTime": mean(data["elapsed"]),
            })

    print(f"Aggregated CSV written to {output_file}")

def final_summary(root_dir: Path, output_file: Path):
    """
    Creates a final summary CSV with the last generation values
    for each classY/nXX combination.
    """

    results = []

    # Iterate over class directories (class1, class2, ...)
    for class_dir in root_dir.glob("class*"):
        if not class_dir.is_dir():
            continue

        # Iterate over nXXX folders inside each class
        for n_dir in class_dir.glob("n*"):
            summary_csv = n_dir / "averages.csv"
            if not summary_csv.exists():
                continue

            # Read CSV with averages per generation
            with summary_csv.open("r", encoding="utf-8") as f:
                reader = csv.DictReader(f, delimiter=";")
                rows = list(reader)
                if not rows:
                    continue

                # Take the last generation row
                last = rows[-1]

                # Append final values to results
                results.append({
                    "Class": class_dir.name,
                    "N": n_dir.name,
                    "AverageBest": last["AverageBest"],
                    "AverageTime": last["AverageTime"]
                })

    # Ensure output directory exists
    output_file.parent.mkdir(parents=True, exist_ok=True)

    # Write final summary CSV
    with output_file.open("w", newline="", encoding="utf-8") as f:
        writer = csv.DictWriter(
            f,
            fieldnames=["Class", "N", "AverageBest", "AverageTime"],
            delimiter=";"
        )
        writer.writeheader()
        writer.writerows(results)

    print(f"Final summary written to: {output_file}")


# -----------------------------
# Entry point
# -----------------------------
if __name__ == "__main__":
    parser = argparse.ArgumentParser(
        description="Batch runner for C# packing solver."
    )

    parser.add_argument(
        "--input-dir",
        default="..\\data\\mpv\\medium",
        help="Directory containing input JSON files."
    )

    parser.add_argument(
        "--output-dir",
        default="..\\experiments\\results\\phc_10_98_medium",
        help="Directory where output files will be written."
    )

    parser.add_argument(
        "--solver_exe",
        default="..\\src\\App\\bin\\Release\\net8.0-windows\\App.exe",
        help="Path to the compiled C# solver executable."
    )

    parser.add_argument(
        "--packing-setting",
        default="..\\experiments\\settings\\packing\\all_heuristics__no_order.json",
        help="Path to JSON file describing PackingSetting."
    )

    parser.add_argument(
        "--evolution-setting",
        default="..\\experiments\\settings\\evolution\\phc__10_98.json",
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