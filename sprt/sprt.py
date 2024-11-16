from subprocess import run
from time import time

t0 = time()

run(
    '"sprt/fastchess.exe" '
    # '-sprt alpha=0.05 beta=0.05 elo0=0 elo1=5 '
    # '-engine cmd="sprt/pitfall v1.0.2.exe" name="Pitfall" '
    # '-engine cmd=sprt/random.exe name="Random" '
    '-engine cmd=sprt/random.exe name="Random 1" '
    '-engine cmd=sprt/random.exe name="Random 2" '
    '-openings file="sprt/book.pgn" format=pgn '
    # '-each tc=8+0.08 -rounds 22939293040 '
    '-each tc=3600+1 -rounds 5000 '
    '-concurrency 4 '
    # '-log file=sprt/log.txt'
)

time_taken = time() - t0

from datetime import timedelta as td

print(f"Time taken: {td(seconds = time_taken)}")