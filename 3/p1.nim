import std/rdstdin
import std/strutils
import std/bitops

func readStdin(): string =
    var txt = ""
    while true:
        var line = ""
        if not rdstdin.readLineFromStdin("", line):
            break
        txt &= line & "\n"
    
    txt[0..^2]

func calculateRates(strings: seq[string]): (uint, uint) =
    let bitCount = strings[0].len - 1

    var gammaRate: uint = 0
    var epsilonRate: uint = 0
    for i in 0..bitCount:
        var count = 0
        for binary in strings:
            if binary[i] == '1':
                count += 1
            else:
                count -= 1

        if count > 0:
            gammaRate.setBit(bitCount - i)
        else:
            epsilonRate.setBit(bitCount - i)

    (gammaRate, epsilonRate)

let split = readStdin().split({'\n'})
let (gr, er) = calculateRates(split)
echo gr * er