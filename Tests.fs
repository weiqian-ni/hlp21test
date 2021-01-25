module Tests
open Expecto
//-----------------------------------------------------------------------------------------------//
//------------------------------DO NOT CHANGE THIS FILE------------------------------------------//
//-----------------------------------------------------------------------------------------------//


open FsCheck

type Mark = {OutOf: float; Attained: float}



let mutable marksSoFar: Map<string,Mark> = Map.empty

let mark qStr m = 
    marksSoFar <- 
        match Map.tryFind qStr marksSoFar with
        | None -> Map.add qStr {OutOf=1.; Attained=m} marksSoFar
        | Some mRec -> Map.add qStr {mRec with Attained = min mRec.Attained m} marksSoFar

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

(*
Q3. The output list is twice the length of the input list. Each input list element occurs in order
in the output, twice. E.g [1;2;5] -> [1;1;2;2;5;5].
You are not allowed to use list indexing (.[] or List.item) in your answer.
*)
let q3Model (lst: 'a list) : 'a list =
    List.init (lst.Length*2) (fun i -> lst.[i/2])



(*
Q4. The output is the sum of all the elements in the input lists.
Recursive functions are not alowed in the answer.
*)
let rec q4Model (lsts: int list list) : int = 
    let rec sumL lst =
        match lst with
        | [] -> 0
        | n :: lst' -> n + sumL lst'

    match lsts with
    | [] -> 0
    | lst :: lsts' -> q4Model lsts' + sumL lst

(*
Q5. The output is the mode (element with maximum number of occurences) in the input list.
Thus [1;2;0;3;2;1;6;6;1] -> 1
If there is more than modal element the output should be the most positive of all such.
You must implement this without using Map types.
You may assume the input list is non-empty - your function is allowed to fail if given an empty list.
*)
let q5Model (lst: int list)  =
    let mapAddOne m (k:int) = 
        Map.tryFind k m
        |> Option.map (fun n -> n+1)
        |> Option.defaultValue 1
        |> (fun x -> Map.add k x m)
    (Map.empty,lst) ||> List.fold mapAddOne
    |> Map.toList
    |> List.map (fun (a,b) -> (b,a))
    |> List.sortDescending
    |> List.head
    |> snd
    

(* 
Q6. List elements are numbered from 0.
Element n in the output list is the product of elements 2n and 2n+1 in the input list
If the input list has an odd number of elements then the last element of the output list
is the square of the last elemnt of the input list.
You are not allowed to use list indexing (.[] or List.item) in your answer.
*)
let rec q6Model (lst: int list) : int list =
    match lst.Length with
    | n when n % 2 = 0 -> 
        List.init (n/2) (fun i -> lst.[2*i] * lst.[2*i+1])
    | n -> 
        q6Model lst.[0..n-2] @ [lst.[n-1] * lst.[n-1]]

[<Tests>]    
let tests =
    testList "Practice Test 1" [
        checkMCQ [0;1] Answers.q1 "Q1"
        checkMCQ [0;1;2] Answers.q2 "Q2"
        checkAgainstModel q3Model Answers.q3 "Q3" 1.
        checkAgainstModel q4Model Answers.q4 "Q4" 1.
        checkAgainstModelIfNotEmpty q5Model Answers.q5 "Q5" 1.
        checkAgainstModel q6Model Answers.q6 "Q6" 1.

    ]


