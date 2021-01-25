// Learn more about F# at http://fsharp.org

open System
open Expecto
open TestLib



[<EntryPoint>]
let main argv =
    // check we are running under F# 5.0
    checkVersionAndWarn()
    printfn "Starting tests of answers in 'answers.fs"
    let defConfig = Expecto.Impl.ExpectoConfig.defaultConfig
    Expecto.Tests.runTestsInAssembly {defConfig with runInParallel = false} [||] |> ignore
    printMarks TestLib.marksSoFar |> ignore
    0
