#!/usr/bin/env -S dotnet fsi --exec

// get an int list
let raw = fsi.CommandLineArgs[1].Split()
          |> Array.map int
          |> Array.toList

// triples
raw
|> List.skip 2
|> List.mapi (fun i e -> raw[i] + raw[i + 1] + e)

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