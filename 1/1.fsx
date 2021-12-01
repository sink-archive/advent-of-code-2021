#!/usr/bin/env -S dotnet fsi --exec

// get an int list
fsi.CommandLineArgs.[1].Split()
|> Array.map int
|> Array.toList

// count increases
|> List.fold
    (fun (prev, incs) next ->
        match prev with
        | None -> (Some(next), incs)
        | Some (p) ->
            if next > p then
                (Some(next), incs + 1)
            else
                (Some(next), incs))
    (None, 0)

// discard the state prev value
|> snd
|> printfn "%i"
