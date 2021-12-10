#!/usr/bin/env -S dotnet fsi --exec

open System
open System.IO

let readStdin () =
    let sr =
        new StreamReader(Console.OpenStandardInput())

    let input = sr.ReadToEnd()
    sr.Dispose()
    input

let isLowPoint (grid: 'a[][]) x y =
    let left =
        if x = 0 then
            true
        else
            grid[y][x - 1] > grid[y][x]
    
    let right =
        if x + 1 = grid[y].Length then
            true
        else
            grid[y][x + 1] > grid[y][x]
    
    let top =
        if y = 0 then
            true
        else
            grid[y - 1][x] > grid[y][x]
            
    let bottom =
        if y + 1 = grid.Length then
            true
        else
            grid[y + 1][x] > grid[y][x]
            
    top && bottom && left && right
    

let grid =
    readStdin().Split("\n", StringSplitOptions.RemoveEmptyEntries)
    // parse to int grid
    |> Array.map (fun r -> r.ToCharArray() |> Array.map (string >> int))

// process low points
grid
|> Array.mapi
    (fun y r ->
        r
        |> Array.mapi (fun x c -> if isLowPoint grid x y then c + 1 else 0))
|> Array.concat
|> Array.sum
|> printfn "%i"