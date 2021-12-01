#!/usr/bin/env -S dotnet fsi --exec

let countIncreases (list: int list) =
    let rec countRec list increases prev =
        match list with
        | [] -> increases
        | head::tail ->
            if head > prev then
                countRec tail (increases + 1) head
            else
                countRec tail increases head
    
    countRec list.Tail 0 list.Head

fsi.CommandLineArgs[1].Split(" ")
|> Array.map int
|> Array.toList
|> countIncreases
|> printfn "%i"