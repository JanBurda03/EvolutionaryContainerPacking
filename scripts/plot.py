import os
import argparse
import pandas as pd
import matplotlib.pyplot as plt

CLASSES = ["class1", "class2", "class3", "class4"]
SIZES = ["n50", "n100", "n150"]


def load_data(base_path, cls, size):
    file_path = os.path.join(base_path, cls, size, "averages.csv")
    if not os.path.exists(file_path):
        print(f"Warning: Missing {file_path}")
        return None
    df = pd.read_csv(file_path, sep=";")
    return df


def plot_results(algorithms, labels, output_path, show_plot=False):
    fig, axes = plt.subplots(len(CLASSES), len(SIZES), figsize=(15, 12), sharex=True, sharey=False)

    for i, cls in enumerate(CLASSES):
        for j, size in enumerate(SIZES):
            ax = axes[i][j]

            for algo_path, label in zip(algorithms, labels):
                df = load_data(algo_path, cls, size)
                if df is None:
                    continue

                ax.plot(df["Generation"], df["AverageBest"], label=label)

            if i == 0:
                ax.set_title(size)
            if j == 0:
                ax.set_ylabel(cls)

            ax.grid(True)

    handles, labels = axes[0][0].get_legend_handles_labels()
    fig.legend(handles, labels, loc="upper center", ncol=len(algorithms))

    fig.supxlabel("Generation")
    fig.supylabel("Average Best Fitness")

    plt.tight_layout(rect=[0, 0, 1, 0.95])

    plt.savefig(output_path, dpi=300)
    print(f"Saved plot to {output_path}")

    if show_plot:
        plt.show()


def main():
    parser = argparse.ArgumentParser(description="Plot evolutionary algorithm comparison.")
    
    parser.add_argument(
        "--algorithms",
        nargs="+",
        default=[
            "C:\\Users\\Jan Burda\\Desktop\\EvolutionaryContainerPacking\\experiments\\results\\elitist\\heavy\\lower_bound_heavy",
            "C:\\Users\\Jan Burda\\Desktop\\EvolutionaryContainerPacking\\experiments\\results\\hc\\hc__1_heavy",
            "C:\\Users\\Jan Burda\\Desktop\\EvolutionaryContainerPacking\\experiments\\results\\elitist\\heavy\\heavy_all_heuristics_no_order",
           "C:\\Users\\Jan Burda\\Desktop\\EvolutionaryContainerPacking\\experiments\\results\\elitist\\heavy\\heavy_all_heuristics_volume",
            "C:\\Users\\Jan Burda\\Desktop\\EvolutionaryContainerPacking\\experiments\\results\\elitist\\heavy\\heavy_all_heuristics_weight",
        ],
        help="Paths to algorithm result folders"
    )

    parser.add_argument(
        "--labels",
        nargs="+",
        default=["Lower Bound", "Hill Climbing", "Elitist - All heuristics",  "Elitist - All heuristics - Volume", "Elitist - All heuristics - Weight"],
        help="Labels for algorithms (optional)"
    )

    parser.add_argument(
        "--output",
        default="comparison-heavy.png",
        help="Output PNG file"
    )

    parser.add_argument(
        "--show",
        action="store_true",
        help="Show plot interactively"
    )

    args = parser.parse_args()

    labels = args.labels if args.labels else args.algorithms

    plot_results(args.algorithms, labels, args.output, args.show)


if __name__ == "__main__":
    main()