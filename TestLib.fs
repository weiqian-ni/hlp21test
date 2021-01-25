module TestLib
open Expecto
open FsCheck

type Mark = {OutOf: float; Attained: float}


let getFSharpVersion() =
    let assembly = System.Reflection.Assembly.Load "FSharp.Core"
    let fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo assembly.Location;
    fvi.FileMajorPart,fvi.FileMinorPart

let printMarks marksSoFar =
    printfn "\nMarks from code testing\nQuestion Weight Mark\n--------------------"
    marksSoFar
    |> Map.toList
    |> List.sort
    |> List.map (fun (qName, {OutOf = o; Attained = a}) ->
                    let a' = max a 0. // initial -1. marks should be zero
                    printfn "%8s %6.1f %4.1f" qName o (max a 0.))

let checkVersionAndWarn() =
    match getFSharpVersion() with
    | major, _ when major >= 5 -> 
        () // latest version
    | major, minor -> 
        printfn "You are running this testbench under F# %d.%d when F# 5.0 or higher is required." major minor
        printfn "The testbench will be run, but results may be incorrect. Please upgrade to F# 5.0"
        printfn "Press any key to continue..."
        System.Console.ReadKey() |> ignore




let mutable marksSoFar: Map<string,Mark> = Map.empty

let mark qStr m = 
    marksSoFar <- 
        match Map.tryFind qStr marksSoFar with
        | None -> 
            {OutOf=1.; Attained=m}
        | Some mRec when mRec.Attained < 0. -> 
            {mRec with Attained = m}
        | Some mRec ->
            {mRec with Attained = min mRec.Attained m}
        |> (fun mRec -> Map.add qStr mRec marksSoFar)

/// This Test always succeeds but prints out a warning if the MCQ answer is not one of the allowed options
let checkMCQ opts q qStr = 
    match (try Ok (q()) with | e -> Error e.Message) with
    | Error mess ->
        printfn "\n****Warning**** %s has not yet been answered\n\n" qStr
    | Ok q -> 
        if not <| List.contains q opts
            then printfn "\n****Warning**** %s must be one of: %s\n\n" qStr (String.concat "," (List.map (sprintf "%A") opts))
    testCase (sprintf "Checking MCQ answer for %s is in allowed range" qStr) <| fun () -> ()
        

let markMCQ correct q qStr = Expect.equal  correct q $"{qStr} answer is {correct}"


let markAndTest markIfOk model q qStr arg  =
    marksSoFar <- Map.add qStr {OutOf=1.; Attained= -1.} marksSoFar
    let ok = model arg = q arg
    mark qStr  (if ok then markIfOk else 0.)
    if not ok then
        printfn $"----Error Detail----\n{qStr} {arg} = {q arg}, {qStr}Model {arg} = {model arg}\n--------------------\n"
        failwithf "test failed"
    ok

let checkAgainstModel model q qStr maxMark =
    testProperty qStr <| markAndTest maxMark model q qStr

let checkAgainstModelIfNotEmpty model q qStr maxMark =
    let markAndTest' markIfOk arg  =       
        arg <> [] ==> 
            Prop.ofTestable (fun () ->
                markAndTest maxMark model q qStr arg)
     

    testProperty qStr <| markAndTest' maxMark

