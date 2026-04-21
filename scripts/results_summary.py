import argparse
import csv
from pathlib import Path


def merge_summaries(root_dir: Path, output_file: Path):
    """
    Merge all summary.csv files into one CSV.

    First two columns: Class and N (from the first summary file)
    Then all AverageBest columns for each folder
    Then all AverageTime columns for each folder
    """

    root_dir = Path(root_dir)

    # find all summary.csv files
    summary_files = sorted(root_dir.rglob("summary.csv"))

    if not summary_files:
        print("No summary.csv files found")
        return

    print(f"Found {len(summary_files)} summary files")

    # First, extract Class and N from the first summary file
    class_column = []
    n_column = []
    for row in csv.DictReader(summary_files[0].open("r", encoding="utf-8"), delimiter=";"):
        class_column.append(row["Class"])
        n_column.append(row["N"])

    # Extract AverageBest and AverageTime from all summary files
    data_by_folder = {}
    for summary_path in summary_files:
        folder_name = summary_path.parent.name
        best_values = []
        time_values = []

        with summary_path.open("r", encoding="utf-8") as f:
            reader = csv.DictReader(f, delimiter=";")
            for row in reader:
                best_values.append(row["AverageBest"])
                time_values.append(row["AverageTime"])

        data_by_folder[folder_name] = {"Best": best_values, "Time": time_values}

    # Create output CSV
    output_file.parent.mkdir(parents=True, exist_ok=True)
    with output_file.open("w", newline="", encoding="utf-8") as f:
        writer = csv.writer(f, delimiter=";")

        # Header: Class, N, then all Best columns, then all Time columns
        header = ["Class", "N"]
        # Best columns first
        for folder in data_by_folder.keys():
            header.append(f"AverageBest_{folder}")
        # Time columns next
        for folder in data_by_folder.keys():
            header.append(f"AverageTime_{folder}")
        writer.writerow(header)

        # Write rows
        num_rows = len(class_column)
        for i in range(num_rows):
            row = [class_column[i], n_column[i]]
            # add Best values
            for folder in data_by_folder.keys():
                row.append(data_by_folder[folder]["Best"][i])
            # add Time values
            for folder in data_by_folder.keys():
                row.append(data_by_folder[folder]["Time"][i])
            writer.writerow(row)

    print(f"Merged file saved to: {output_file}")


# -----------------------------
# Entry point
# -----------------------------
if __name__ == "__main__":
    parser = argparse.ArgumentParser(
        description="Merge all summary.csv files into a single file with Class, N, AverageBest and AverageTime."
    )

    parser.add_argument(
        "--root_dir",
        default="..\\experiments\\results\\hc",
        help="Root folder where summary.csv files will be searched"
    )

    parser.add_argument(
        "--output",
        default="results.csv",
        help="Name of the output CSV file"
    )

    args = parser.parse_args()

    root = Path(args.root_dir)

    merge_summaries(
        root,
        root / args.output
    )