from subprocess import run

run(
    '"sprt/fastchess.exe" -engine cmd="sprt/pitfall v1.0.2.exe" name="Pitfall" -engine cmd=sprt/random.exe name="Random" -openings file="sprt/book.pgn" format=pgn -each tc=8+0.08 -rounds 2 -repeat -concurrency 4 -log file=sprt/log.txt'
)