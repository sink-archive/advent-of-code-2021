import std/rdstdin
import std/strutils
import std/parseutils
import std/sequtils
import sugar

func readStdin(): string =
    var txt = ""
    while true:
        var line = ""
        if not rdstdin.readLineFromStdin("", line):
            break
        txt &= line & "\n"
    
    txt[0..^2]

func ratingPass(strings: seq[string], bit: int, co2: bool): seq[string] =
    var count = 0
    for binary in strings:
        if binary[bit] == '1':
            count += 1
        else:
            count -= 1
    
    var targetBit = '0'
    # count is positive if more 1s, and negative if more 0s, and 0 if equal
    if count >= 0:
        targetBit = '1'
    
    # co2 does the opposite to oxygen generator
    if co2:
        if targetBit == '1':
            targetBit = '0'
        else:
            targetBit = '1'
    
    strings.filter((x) => x[bit] == targetBit)

func calculateRating(strings: seq[string], co2: bool): uint =
    var substrings = strings

    let bitCount = substrings[0].len - 1
    for i in 0..bitCount:
        substrings = ratingPass(substrings, i, co2)
        if substrings.len == 1:
            var x: uint = 0
            discard substrings[0].parseBin(x)
            return x

let split = readStdin().split({'\n'})
echo calculateRating(split, false) * calculateRating(split, true)