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

let rawlines = readStdin().split("\n")
var lines = newSeq[((int, int), (int, int))]()
for line in rawlines:
    let spl = line.split(" -> ")
    let sc1 = spl[0].split(",")
    let sc2 = spl[1].split(",")
    let c1 = (sc1[0].parseInt(), sc1[1].parseInt())
    let c2 = (sc2[0].parseInt(), sc2[1].parseInt())
    
    lines.add((c1, c2))

var gridX = 0
var gridY = 0
for line in lines:
    gridX = max(gridX, max(line[0][0], line[1][0]))
    gridY = max(gridY, max(line[0][1], line[1][1]))

# intialize grid as all 0
var grid = newSeqWith[seq[int]](gridY + 1, newSeqWith[int](gridX + 1, 0))

for rline in lines:
    var line = rline

    if line[0][0] == line[1][0]:
        if line[0][1] > line[1][1]:
            line = (line[1], line[0])
        
        # line has equal x coords, so is vertical
        for y in line[0][1]..line[1][1]:
            grid[y][line[0][0]] += 1
    elif line[0][1] == line[1][1]:
        if line[0][0] > line[1][0]:
            line = (line[1], line[0])
        
        # line has equal y coords, so is horizontal
        for x in line[0][0]..line[1][0]:
            grid[line[0][1]][x] += 1
    else:
        # line has unequal x and y coords, so is diagonal

        if line[0][0] > line[1][0]:
            line = (line[1], line[0])

        for rx in line[0][0]..line[1][0]:
            let x = rx - line[0][0]
            if line[0][1] < line[1][1]:
                # line is going \ that way
                grid[line[0][1] + x][rx] += 1
            else:
                # line is going / that way
                grid[line[0][1] - x][rx] += 1

var count = 0
for y in 0..gridY:
    for x in 0..gridX:
        if grid[y][x] >= 2:
            count += 1

echo count