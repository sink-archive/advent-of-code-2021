#!/usr/bin/env -S dotnet fsi --exec

open System
open System.IO

// util
let any func list = None <> List.tryFindIndex func list

let readStdin () =
    let sr = new StreamReader(Console.OpenStandardInput())
    let input = sr.ReadToEnd()
    sr.Dispose()
    input

// some stuff to make type signatures more sensible
type cell = int * bool
type board = cell list list

// actual solution
let isBoardWinning (board: board) =
    let rowsFull = board |> any (List.forall snd)
    
    // this is a function not a value hence () - this is so we can get the performance boost of a short circuiting OR
    let columnsFull () =
        // for any of the columns
        [1..(board[0].Length - 1)]
        |> any (fun i ->
            // all rows at the column must be true
            board
            |> List.forall (fun r -> snd r[i]))
    
    rowsFull || columnsFull()

let markNumber num (boards: board list): board list =
    boards
    |> List.map (fun board ->
        board
        |> List.map(fun row ->
            row
            |> List.map (fun cell -> (fst cell, snd cell || fst cell = num)))
        )

let calculateScore (board: board) lastDraw =
    let sumOfUnmarked =
        board
        |> List.concat              // flatten list
        |> List.filter (snd >> not) // only where not marked
        |> List.map fst             // discard if marked or not
        |> List.sum                 // sum all
    
    sumOfUnmarked * lastDraw


// i have officially stopped caring. This is not a robust production app, this is an AoC solver.
#nowarn "0025"
let rec findLoserScore (draws: int list) (boards: board list) =
    match boards with
    | [] -> failwith "Somehow I ran out of boards!"
    | [loser] ->
        // dont ask me why this works but it does
        let [marked] = markNumber draws.Head [loser]
        calculateScore marked draws.Head
    | _ ->
        match draws with
        | [] -> failwith "Ran out of draws!"
        | draw::nextDraws ->
            findLoserScore
                nextDraws
                (boards
                 // mark off numbers on boards
                 |> markNumber draw
                 // remove winning boards
                 |> List.filter (isBoardWinning >> not))

let rawDraws::rawBoards =
    readStdin().Split("\n\n")
    |> Array.toList

let draws =
    rawDraws.Split(",")
    |> Array.toList
    |> List.map int

let boards: board list =
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

let losingScore = findLoserScore draws boards
printfn $"%i{losingScore}"