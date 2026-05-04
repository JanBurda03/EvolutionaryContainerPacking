from __future__ import annotations

import argparse
import csv
import json
import statistics
from pathlib import Path
from typing import Optional

import pandas as pd


CLASSES = ["class1", "class2", "class3", "class4"]
SIZES = ["n50", "n100", "n150"]

DEFAULT_GENERATIONS = 301


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description=(
            "Compute lower-bound fitness values for a dataset and save them "
            "in averages.csv files compatible with the plotting script."
        )
    )
    parser.add_argument(
        "--input-root",
        default="C:\\Users\\Jan Burda\\Desktop\\EvolutionaryContainerPacking\\data\\original",
        help=(
            "Root folder containing class1/class2/class3/class4 directories. "
            "Each class folder must contain n50/n100/n150 subfolders with JSON inputs."
        ),
    )
    parser.add_argument(
        "--output-root",
        default="./lower_bound_heavy",
        help=(
            "Destination root folder where mirrored class/size folders and averages.csv "
            "files will be written."
        ),
    )
    parser.add_argument(
        "--summary-output",
        default="./lower_bound_heavy/summary.csv",
        help=(
            "Path to the aggregated summary CSV built from the last generation "
            "of each averages.csv file."
        ),
    )
    parser.add_argument(
        "--generations",
        type=int,
        default=DEFAULT_GENERATIONS,
        help=(
            "Number of rows to write to each averages.csv. The lower-bound value "
            "is repeated for every generation so the file matches the format of real results."
        ),
    )
    return parser.parse_args()


def iter_json_files(folder: Path):
    if not folder.exists():
        return []
    return sorted(
        p for p in folder.rglob("*.json")
        if p.is_file() and p.name.lower().endswith(".json")
    )


def load_json(path: Path) -> dict:
    with path.open("r", encoding="utf-8") as f:
        return json.load(f)


def compute_instance_lower_bound(data: dict) -> float:
    container = data["ContainerProperties"]
    container_sizes = container["Sizes"]
    container_volume = (
        int(container_sizes["X"]) * int(container_sizes["Y"]) * int(container_sizes["Z"])
    )
    max_weight = int(container["MaxWeight"])

    total_weight = 0
    total_volume = 0

    for box in data.get("BoxPropertiesList", []):
        box_sizes = box["Sizes"]
        total_weight += int(box["Weight"])
        total_volume += (
            int(box_sizes["X"]) * int(box_sizes["Y"]) * int(box_sizes["Z"])
        )

    if container_volume <= 0 or max_weight <= 0:
        raise ValueError("Container volume and max weight must be positive.")

    
    return max(float(total_weight)/max_weight, float(total_volume)/container_volume) + 1


def compute_folder_average(folder: Path) -> Optional[float]:
    json_files = iter_json_files(folder)
    if not json_files:
        return None

    values = []
    for file_path in json_files:
        data = load_json(file_path)
        values.append(compute_instance_lower_bound(data))

    return float(statistics.mean(values))


def write_averages_csv(output_file: Path, generations: int, average_best: float) -> None:
    output_file.parent.mkdir(parents=True, exist_ok=True)

    rows = [
        {
            "Generation": g,
            "AverageBest": average_best,
            "AverageAverage": average_best,
            "AverageTime": 0.0,
        }
        for g in range(generations)
    ]
    df = pd.DataFrame(rows)
    df.to_csv(output_file, sep=";", index=False, float_format="%.10f")


def final_summary(root_dir: Path, output_file: Path) -> None:
    """
    Creates a final summary CSV with the last generation values
    for each class/nXX combination.
    """
    results = []

    for class_dir in root_dir.glob("class*"):
        if not class_dir.is_dir():
            continue

        for n_dir in class_dir.glob("n*"):
            summary_csv = n_dir / "averages.csv"
            if not summary_csv.exists():
                continue

            with summary_csv.open("r", encoding="utf-8") as f:
                reader = csv.DictReader(f, delimiter=";")
                rows = list(reader)
                if not rows:
                    continue

                last = rows[-1]

                results.append({
                    "Class": class_dir.name,
                    "N": n_dir.name,
                    "AverageBest": last["AverageBest"],
                    "AverageAverage": last.get("AverageAverage", last["AverageBest"]),
                    "AverageTime": last.get("AverageTime", "0.0"),
                })

    results.sort(key=lambda x: (x["Class"], x["N"]))

    output_file.parent.mkdir(parents=True, exist_ok=True)
    with output_file.open("w", newline="", encoding="utf-8") as f:
        writer = csv.DictWriter(
            f,
            fieldnames=["Class", "N", "AverageBest", "AverageAverage", "AverageTime"],
            delimiter=";"
        )
        writer.writeheader()
        writer.writerows(results)

    print(f"Final summary written to: {output_file}")


def main() -> None:
    args = parse_args()
    input_root = Path(args.input_root).resolve()
    output_root = Path(args.output_root).resolve()
    summary_output = Path(args.summary_output).resolve()

    if not input_root.exists():
        raise FileNotFoundError(f"Input root does not exist: {input_root}")

    if args.generations <= 0:
        raise ValueError("--generations must be a positive integer.")

    for cls in CLASSES:
        for size in SIZES:
            folder = input_root / cls / size
            if not folder.exists():
                print(f"Warning: missing folder {folder}")
                continue

            average_lb = compute_folder_average(folder)
            if average_lb is None:
                print(f"Warning: no JSON files found in {folder}")
                continue

            out_csv = output_root / cls / size / "averages.csv"
            write_averages_csv(out_csv, args.generations, average_lb)
            print(f"Wrote {out_csv} (AverageBest = {average_lb:.10f})")

    final_summary(output_root, summary_output)


if __name__ == "__main__":
    main()