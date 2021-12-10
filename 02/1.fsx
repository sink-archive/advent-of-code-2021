#!/usr/bin/env -S dotnet fsi --exec

// get a list
let instructions =
    fsi.CommandLineArgs[1].Split("\n")
    |> Array.toList
    |> List.map (fun s ->
        match s.Split(" ") |> Array.toList with
        | [ dir; num ] -> (dir, int num)
        | _ -> failwith "invalid instruction")

let rec calculatePos depth horiz instructions =
    match instructions with
    | [] -> (depth, horiz)
    | (dir, num) :: tail ->
        match dir with
        | "up"      -> calculatePos (depth - num) horiz tail
        | "down"    -> calculatePos (depth + num) horiz tail
        | "forward" -> calculatePos depth (horiz + num) tail
        | _ -> failwith "invalid direction"

let pos = calculatePos 0 0 instructions
printfn $"%i{fst pos * snd pos}"