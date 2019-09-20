module Comp2

open Elmish

type Model = { Comp2: string }

type Msg =
    | SomeComp2Msg

let init () =
    { Comp2 = "" }

let update (state:State.IStateManager) (msg:Msg) (model:Model) =
    let shared = state.State.State
    match msg with
    | SomeComp2Msg ->
        state.StartDecrement ()
        { model with Comp2 = string shared + "_" + model.Comp2 }, Cmd.none
    //let shared = state.State.State
    //let cmd = Cmd.ofSub(fun dispatch ->
    //    // Works, but order might change...
    //    dispatch(SomeComp2Msg)
    //    state.StartIncrement ()
    //    dispatch(SomeComp2Msg))
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
      [ Content.content [] [ str ("Comp 2 (" + model.Comp2 + ")") ]
        State.button "Comp2" (fun _ -> dispatch SomeComp2Msg)
        State.view state.State state.Dispatch ]