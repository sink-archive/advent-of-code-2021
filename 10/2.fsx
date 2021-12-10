#!/usr/bin/env -S dotnet fsi --exec

open System
open System.IO

let readStdin () =
    let sr =
        new StreamReader(Console.OpenStandardInput())

    let input = sr.ReadToEnd()
    sr.Dispose()
    input

type Bracket = Paren | Square | Brace | Angle

let charToBracket =
    function
    | '(' | ')' -> Paren
    | '[' | ']' -> Square
    | '{' | '}' -> Brace
    | '<' | '>' -> Angle
    | _ -> failwith "Invalid char to convert to bracket"

let score =
    function
    | Paren -> 1UL
    | Square -> 2UL
    | Brace -> 3UL
    | Angle -> 4UL

let scanForError line =
    let rec complete bracketStack working =
        match bracketStack with
        | [] -> working
        | head::tail ->
            complete tail ((working * 5UL) + score head)

    let rec scanRec line bracketStack =
        match line with
        | [] -> complete bracketStack 0UL

        | current::ctail ->
            match current with
            | '(' | '[' | '{' | '<' -> 
                scanRec ctail ((charToBracket current)::bracketStack)
            | ')' | ']' | '}' | '>' ->
                match bracketStack with
                | [] -> failwith "Ran out of brackets on stack"
                | lastBracket::btail ->
                    if (charToBracket current) <> lastBracket then
                        0UL // ignore error lines
                    else
                        // line was valid - just keep going
                        scanRec ctail btail
            
            | _ -> failwith "Invalid bracket in chunk"

    scanRec (line |> Seq.toList) []

let sorted =
    readStdin().Split("\n", StringSplitOptions.RemoveEmptyEntries)
    |> Array.map scanForError
    |> Array.filter (fun x -> x <> 0UL)
    |> Array.sort

printfn $"%i{sorted[sorted.Length / 2]}"