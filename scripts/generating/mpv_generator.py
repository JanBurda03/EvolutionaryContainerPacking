import random
from typing import List
from models import Sizes, BoxProperties, ContainerProperties, PackingInput


def _random_type_box(rng, W, H, D, box_type):

    if box_type == 1:
        w = rng.randint(1, W // 2)
        h = rng.randint(2 * H // 3, H)
        d = rng.randint(2 * D // 3, D)
    elif box_type == 2:
        w = rng.randint(2 * W // 3, W)
        h = rng.randint(1, H // 2)
        d = rng.randint(2 * D // 3, D)
    elif box_type == 3:
        w = rng.randint(2 * W // 3, W)
        h = rng.randint(2 * H // 3, H)
        d = rng.randint(1, D // 2)
    elif box_type == 4:
        w = rng.randint(W // 2, W)
        h = rng.randint(H // 2, H)
        d = rng.randint(D // 2, D)
    elif box_type == 5:
        w = rng.randint(1, W // 2)
        h = rng.randint(1, H // 2)
        d = rng.randint(1, D // 2)



    elif box_type == 6:
        w = rng.randint(1, 10)
        h = rng.randint(1, 10)
        d = rng.randint(1, 10)
    elif box_type == 7:
        w = rng.randint(1, 35)
        h = rng.randint(1, 35)
        d = rng.randint(1, 35)
    elif box_type == 8:
        w = rng.randint(1, 100)
        h = rng.randint(1, 100)
        d = rng.randint(1, 100)
    else:
        raise ValueError("Invalid box type")

    return w, h, d

def _get_size(class_id):
    if class_id <= 5:
        return 100
    if class_id == 6:
        return 10
    if class_id == 7:
        return 40
    if class_id == 8:
        return 100
    raise ValueError("Class must be 1–8")


"For Class k (k = 1...5), each item of type k is chosen with probability 60%, and the other four types with probability 10% each."
def _sample_type_for_class(rng, class_id):
    probs = [0.1] * 5
    probs[class_id - 1] = 0.6
    return rng.choices([1, 2, 3, 4, 5], probs)[0]


def generate_mpv_instance(
    class_id: int,
    box_count: int,
    seed: int
) -> PackingInput:
    rng = random.Random(seed)

    
    W = H = D = _get_size(class_id)
    

    boxes: List[BoxProperties] = []

    for i in range(box_count):
        if class_id <= 5:
            t = _sample_type_for_class(rng, class_id)
        else:
            t = class_id

        w, h, d = _random_type_box(rng, W, H, D, t)

        boxes.append(
            BoxProperties(
                id=i,
                sizes=Sizes(w, h, d),
                weight=1  # MPV benchmark has no weights
            )
        )

    container = ContainerProperties(
        sizes=Sizes(W, H, D),
        max_weight=2**31 - 1
    )

    return PackingInput(container, boxes)
