#!/usr/bin/env -S dotnet fsi --exec

// util

open System.IO

let any func list = list |> List.tryFindIndex func <> None

open System
let readStdin () =
    let sr = new StreamReader(Console.OpenStandardInput())
    let input = sr.ReadToEnd()
    sr.Dispose()
    input

// actual solution
let checkWinningBoard boards =
    boards
    |> List.tryFindIndex
        (fun board ->
            // Check if any rows are full
            board
            // all cells in a row are true
            |> any (fun row -> row |> List.forall snd)
            ||
            // OR Check if any columns are full
            // for any of the columns
            [1..(board[0].Length - 1)]
            |> any (fun i ->
                // all rows at the column must be true
                board
                |> List.forall (fun r -> snd r[i]))
            )

let markNumber boards num =
    boards
    |> List.map (fun board ->
        board
        |> List.map(fun row ->
            row
            |> List.map (fun cell -> (fst cell, snd cell || fst cell = num)))
        )

let calculateScore board lastDraw =
    let sumOfUnmarked =
        board
        |> List.concat // flatten list
        |> List.filter (snd >> not) // only where not marked
        |> List.map fst // discard if marked or not
        |> List.sum // sum all
    
    sumOfUnmarked * lastDraw

let rec findWinnerScore draws boards lastDraw =
    match checkWinningBoard boards with
    | Some(index) -> calculateScore boards[index] lastDraw
    | None ->
        match draws with
        | [] -> failwith "Ran out of draws!"
        | draw::nextDraws ->
            findWinnerScore nextDraws (markNumber boards draw) draw

#nowarn "0025"
let rawDraws::rawBoards =
    readStdin().Split("\n\n")
    |> Array.toList

let draws =
    rawDraws.Split(",")
    |> Array.toList
    |> List.map int

let boards =
    rawBoards
    |> List.map
        (fun b ->
            b.Split("\n")
            |> Array.toList
            |> List.map
                (fun r ->
                    r.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    |> Array.toList
                    |> List.map (fun s -> (int s, false))))

let winningScore = findWinnerScore draws boards 0
printfn $"%i{winningScore}"