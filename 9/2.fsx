#!/usr/bin/env -S dotnet fsi --exec

open System
open System.IO

let readStdin () =
    let sr =
        new StreamReader(Console.OpenStandardInput())

    let input = sr.ReadToEnd()
    sr.Dispose()
    input

let rec walk (grid: int[][]) x y =
    if grid[y][x] = 9 then
        (x, y)
    else
        let leftSafe   = x > 0
        let topSafe    = y > 0
        let rightSafe  = x + 1 < grid[y].Length
        let bottomSafe = y + 1 < grid.Length
        
        if leftSafe && grid[y][x - 1] < grid[y][x] then
            walk grid (x - 1) y
        elif rightSafe && grid[y][x + 1] < grid[y][x] then
            walk grid (x + 1) y
        elif topSafe && grid[y - 1][x] < grid[y][x] then
            walk grid x (y - 1)
        elif bottomSafe && grid[y + 1][x] < grid[y][x] then
            walk grid x (y + 1)
        else
            (x, y)

let grid =
    readStdin().Split("\n", StringSplitOptions.RemoveEmptyEntries)
    // parse to int grid
    |> Array.map (fun r -> r.ToCharArray() |> Array.map (string >> int))

grid
// where does each coord in the grid go?
|> Array.mapi
    (fun y r ->
        r
        |> Array.mapi (fun x c -> (walk grid x y, (x, y))))
// flatten
|> Array.concat
// count by coordinate
|> Array.countBy fst
// find the top 3
|> Array.sortByDescending snd
|> Array.take 3
// multiply
|> Array.map snd
|> Array.reduce (*)
|> printfn "%i"