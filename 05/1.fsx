#!/usr/bin/env -S dotnet fsi --exec

open System
open System.IO

let readStdin () =
    let sr =
        new StreamReader(Console.OpenStandardInput())

    let input = sr.ReadToEnd()
    sr.Dispose()
    input

type coord = int * int
type line = coord * coord

let parseCoord (str: string) =
    match str.Split(",") with
    | [| x; y |] -> coord (int x, int y)
    | _ -> failwith "Coord was invalid"

let pointCoveredByLine (line: line) (point: coord) =
    let covered l =
        let inBoundsX =
            fst (fst l) <= fst point
            && fst (snd l) >= fst point

        let inBoundsY =
            snd (fst l) <= snd point
            && snd (snd l) >= snd point

        inBoundsX && inBoundsY

    // line may not be in order
    covered line || covered (snd line, fst line)

let findMaxPoint (lines: line list) =
    lines
    |> List.map (fun l -> [ fst l; snd l ])
    |> List.concat
    |> List.reduce (fun acc next -> (Math.Max(fst acc, fst next), Math.Max(snd acc, snd next)))

let moreThanTwoOverlap lines point =
    let rec overlapRec point lines overlapCount =
        if overlapCount = 2 then
            true
        else
            match lines with
            | [] -> false
            | l :: tail ->
                overlapRec
                    point
                    tail
                    (if pointCoveredByLine l point then
                         overlapCount + 1
                     else
                         overlapCount)

    overlapRec point lines 0

let lines =
    readStdin()
        .Split("\n", StringSplitOptions.RemoveEmptyEntries)
    |> Array.toList
    |> List.map
        (fun pair ->
            match pair.Split(" -> ") with
            | [| a; b |] -> (parseCoord a, parseCoord b)
            | _ -> failwith "Coord pair was invalid")
    |> List.filter
        (fun l ->
            fst (fst l) = fst (snd l)
            || snd (fst l) = snd (snd l))

// NOTE: gridX and gridY are the last INDEXES of the grid - the width and height are 1 greater than these!
let gridX, gridY = findMaxPoint lines

let count =
    [ 0 .. gridY ]
    |> List.mapi
        (fun y _ ->
            [ 0 .. gridX ]
            |> List.mapi (fun x _ -> moreThanTwoOverlap lines (x, y))
            |> List.filter id
            |> List.length)
    |> List.sum

printfn $"%i{count}"