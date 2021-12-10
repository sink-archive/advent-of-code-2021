#!/usr/bin/env -S dotnet fsi --exec

open System
open System.IO

let readStdin () =
    let sr =
        new StreamReader(Console.OpenStandardInput())

    let input = sr.ReadToEnd()
    sr.Dispose()
    input

type Bracket =
    | Paren
    | Square
    | Brace
    | Angle

let charToBracket =
    function
    | '(' -> Paren
    | ')' -> Paren
    | '[' -> Square
    | ']' -> Square
    | '{' -> Brace
    | '}' -> Brace
    | '<' -> Angle
    | '>' -> Angle
    | _ -> failwith "Tried to convert invalid char to a bracket"

let score =
    function
    | Paren -> 3
    | Square -> 57
    | Brace -> 1197
    | Angle -> 25137

let scanForError line =
    let rec scanRec chunks bracketStack =
        match chunks with
        | [] -> 0
        | current::ctail ->
            match current with
            | '(' -> scanRec ctail (Paren::bracketStack)
            | '[' -> scanRec ctail (Square::bracketStack)
            | '{' -> scanRec ctail (Brace::bracketStack)
            | '<' -> scanRec ctail (Angle::bracketStack)
            | ')' ->
                match bracketStack with
                | [] -> failwith "Ran out of brackets on stack"
                | last::btail ->
                    if last = Paren then
                        scanRec ctail btail
                    else
                        score (charToBracket current)
            | ']' ->
                match bracketStack with
                | [] -> failwith "Ran out of brackets on stack"
                | last::btail ->
                    if last = Square then
                        scanRec ctail btail
                    else
                        score (charToBracket current)
            | '}' ->
                match bracketStack with
                | [] -> failwith "Ran out of brackets on stack"
                | last::btail ->
                    if last = Brace then
                        scanRec ctail btail
                    else
                        score (charToBracket current)
            | '>' ->
                match bracketStack with
                | [] -> failwith "Ran out of brackets on stack"
                | last::btail ->
                    if last = Angle then
                        scanRec ctail btail
                    else
                        score (charToBracket current)
            
            | _ -> failwith "Invalid bracket in chunk"

    
    scanRec (line |> Seq.toList) []

readStdin().Split("\n", StringSplitOptions.RemoveEmptyEntries)
|> Array.map scanForError
|> Array.sum
|> printfn "%i"