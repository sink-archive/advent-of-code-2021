import std/rdstdin
import std/strutils
import std/sequtils

func readStdin(): string =
    var txt = ""
    while true:
        var line = ""
        if not rdstdin.readLineFromStdin("", line):
            break
        txt &= line & "\n"
    
    txt[0..^2]

func calculateMoveFuel(crabs: seq[int], moveTo: int): int =
    var fuel = 0
    
    for crab in crabs:
        fuel += abs(crab - moveTo)

    fuel

let crabs = readStdin().split(",").map(parseInt)

var bestFuel = calculateMoveFuel(crabs, 0)
for i in 1..max(crabs):
    bestFuel = min(bestFuel, calculateMoveFuel(crabs, i))

echo bestFuel