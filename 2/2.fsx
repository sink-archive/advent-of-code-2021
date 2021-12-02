#!/usr/bin/env -S dotnet fsi --exec

// get a list
let instructions =
    fsi.CommandLineArgs.[1].Split("\n")
    |> Array.toList
    |> List.map (fun s ->
        match s.Split(" ") |> Array.toList with
        | [ dir; num ] -> (dir, int num)
        | _ -> failwith "invalid instruction")

let rec calculatePos depth horiz aim instructions =
    match instructions with
    | [] -> (depth, horiz)
    | (dir, num) :: tail ->
        match dir with
        | "up"      -> calculatePos depth horiz (aim - num) tail
        | "down"    -> calculatePos depth horiz (aim + num) tail
        | "forward" -> calculatePos (depth + aim * num) (horiz + num) aim tail
        | _ -> failwith "invalid direction"

let pos = calculatePos 0 0 0 instructions
printfn $"%i{fst pos * snd pos}"