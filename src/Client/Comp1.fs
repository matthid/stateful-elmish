module Comp1


open Elmish

type Model = { Comp1: string }

type Msg =
    | SomeComp1Msg

let init () =
    { Comp1 = "" }

let update (state:State.IStateManager) (msg:Msg) (model:Model) =
    let shared = state.State.State
    match msg with
    | SomeComp1Msg ->
        state.StartDecrement ()
        { model with Comp1 = string shared + "_" + model.Comp1 }, Cmd.none
    
    //let cmd = Cmd.ofSub(fun dispatch ->
    //    // Works, but order might change...
    //    dispatch(SomeComp1Msg)
    //    state.StartDecrement ()
    //    dispatch(SomeComp1Msg))
    //// Works    
    //state.StartIncrement()
    //model, cmd

open Elmish.React
open Fable.React
open Fable.React.Props
open Fetch.Types
open Thoth.Fetch
open Fulma
open Thoth.Json

let view (state:State.IStateManager) (model:Model) (dispatch:Dispatch<Msg>) =
    Container.container []
      [ Content.content [] [ str  ("Comp 1 (" + model.Comp1 + ")") ]
        State.button "Comp1" (fun _ -> dispatch SomeComp1Msg)
        State.view state.State state.Dispatch ]

    