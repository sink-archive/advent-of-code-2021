#!/usr/bin/env -S dotnet fsi --exec

open System
open System.IO

let flip (x, y) = (y, x)

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

let pointCoveredBy90Line (line: line) (point: coord) =
    let covered l =
        let sameXOrY =
            // same y coord as both line vertices
            (snd (fst l) = snd point
            && snd (snd l) = snd point)
            ||
            // same x coord as both line vertices
            (fst (fst l) = fst point
            && fst (snd l) = fst point)
            
        let inBoundsX =
            // inside range of x values
            fst (fst l) <= fst point
            && fst (snd l) >= fst point

        let inBoundsY =
            // inside range of y values
            snd (fst l) <= snd point
            && snd (snd l) >= snd point

        inBoundsX && inBoundsY && sameXOrY

    // line may not be in order
    (covered line || covered (flip line))

let pointCoveredBy45DownLine (line: line) (point: coord) =
    let rec covered l p =
        // line is facing \ this way
        fst (fst l) < fst (snd l)
        && snd (fst l) < snd (snd l)
        // we arent past the end of the line
        && fst p <= fst (snd l)
        // rec and check
        && if fst p < fst (fst l) then
               // do this check so that we dont start before the first point of the line and recurse off forever
               false
           elif fst p = fst (fst l) then
               // either we hit the line vertex or we didnt
               snd p = snd (fst l)
           else
               covered l (fst p - 1, snd p - 1)

    covered line point || covered (flip line) point

let pointCoveredBy45UpLine (line: line) (point: coord) =
    let rec covered l p =
        // line is facing / this way
        fst (fst l) < fst (snd l)
        && snd (fst l) > snd (snd l)
        // we arent past the end of the line
        && fst p <= fst (snd l)
        // rec and check
        && if fst p < fst (fst l) then
               // do this check so that we dont start before the first point of the line and recurse off forever
               false
           elif fst p = fst (fst l) then
               // either we hit the line vertex or we didnt
               snd p = snd (fst l)
           else
               covered l (fst p - 1, snd p + 1)

    covered line point || covered (flip line) point

let pointCoveredByLine line point =
    pointCoveredBy90Line line point
    || pointCoveredBy45DownLine line point
    || pointCoveredBy45UpLine line point

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

// NOTE: gridX and gridY are the last INDEXES of the grid - the width and height are 1 greater than these!
let gridX, gridY = findMaxPoint lines

// oh my god diagonal line checks are SO SLOWWWWW
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